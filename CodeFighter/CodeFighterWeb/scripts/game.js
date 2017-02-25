/// <reference path="scripts/render.js"/>
/// <reference path="scripts/utility.js"/>
/// <reference path="scripts/sprite.js"/>
/// <reference path="scripts/resources.js"/>

// GLOBALS
var canvas;
var ctx;

// magic numbers for height and width of canvas
var gameH = 800;
var gameW = 800;

// magic number for height/width of each 'cell' of the grid.
var cellSize = 32;

// facing directions for sprites
var dir = ['W', 'NW', 'N', 'NE', 'E', 'SE', 'S', 'SW'];

// collections of things to render
var backgroundTiles = [];
var ships = [];
var beamShots = [];
var explosions = [];

// magic number for ship movement speed
var shipSpeed = 32;

// magic numbers for beam weapon animations
var rndBeamDeviation = 10;
var defaultBeamDuration = 0.4;
var defaultBeamRenderDelay = 0.3;
var beamRenderDelayMaxRange = 0.3;
var beamMissThickness = 1;
var beamHitThickness = 2;
var beamCritThickness = 3;
var beamFriendlyColor = 'rgba(255,0,0,0.0)';
var beamEnemyColor = 'rgba(0,255,0,0.0)';

// flags to determine if the next action should be pulled
var shipsDone = true;
var beamsDone = true;
var explosionsDone = true;
var maybeDone = false;

// last time the game loop was run, for calculating delta-time
var lastTime = Date.now();

// action queue
var actions = null;

// start/pause variable
var keepGoing = false;


document.addEventListener('DOMContentLoaded', function () {
    // get canvas
    canvas = document.getElementById('mainCanvas');
    // get context
    ctx = canvas.getContext('2d');

    // set the height & width
    canvas.width = gameW;
    canvas.height = gameH;

    // load all resources
    resources.load([
		'images/Galaxy_star_field1.png',
		'images/Galaxy_star_field2.png',
		'images/Galaxy_star_field3.png',
		'images/Galaxy_star_field4.png',
        'images/ship2.png',
        'images/enemy2.png',
        'images/explosion.png'
    ]);
    resources.onReady(init);
});

//BUTTONS
function start() {
    keepGoing = true;
    startGame();
    var startButton = document.getElementById("btnStart");
    startButton.className += "hidden";
    var pauseButton = document.getElementById("btnPause");
    pauseButton.className = pauseButton.className.replace(/(?:^|\s)hidden(?!\S)/g, '');
}
function pause() {
    keepGoing = false;
    var pauseButton = document.getElementById("btnPause");
    pauseButton.className += "hidden";
    var resumeButton = document.getElementById("btnResume");
    resumeButton.className = resumeButton.className.replace(/(?:^|\s)hidden(?!\S)/g, '');
}
function resume() {
    var now = Date.now();
    keepGoing = true;
    var resumeButton = document.getElementById("btnResume");
    resumeButton.className += "hidden";
    var pauseButton = document.getElementById("btnPause");
    pauseButton.className = pauseButton.className.replace(/(?:^|\s)hidden(?!\S)/g, '');
}
function resetButtons() {
    if (!document.getElementById("btnResume").className.match(/(?:^|\s)hidden(?!\S)/)) {
        var resumeButton = document.getElementById("btnResume");
        resumeButton.className += "hidden";
    }
    if (!document.getElementById("btnPause").className.match(/(?:^|\s)hidden(?!\S)/)) {
        var pauseButton = document.getElementById("btnPause");
        pauseButton.className += "hidden";
    }
    if (document.getElementById("btnStart").className.match(/(?:^|\s)hidden(?!\S)/)) {
        var startButton = document.getElementById("btnStart");
        startButton.className = startButton.className.replace(/(?:^|\s)hidden(?!\S)/g, '');
    }
    keepGoing = false;
}

// RANDOMIZER
function rollDice(sides) {
    return Math.floor(Math.random(new Date().getMilliseconds()) * sides + 1);
}

function rollFloat(max) {
    return Math.random(new Date().getMilliseconds()) * max;
}



// INIT
function init() {
    initBackground();
    renderAll();
}
function initBackground() {
    var index = 0;
    for (var x = 0; x < gameW / cellSize; x++) {
        for (var y = 0; y < gameH / cellSize; y++) {
            var rand = rollDice(4);
            var imageName = 'images/Galaxy_star_field' + rand + '.png';
            backgroundTiles[index] = {
                pos: [x * cellSize, y * cellSize],
                sprite: new Sprite(imageName, [0, 0], [cellSize, cellSize])
            }
            index++;
        }
    }
}



// ACTIONS
function getActions() {
    // TODO: fetch actions from web service
    loadJSON(function (response) {
        actions = JSON.parse(response);
        gameLoop();
    });


}

function loadJSON(callback) {

    var xobj = new XMLHttpRequest();
    xobj.overrideMimeType('application/json');
    xobj.open('GET', 'scripts/actions.json', true);
    xobj.onreadystatechange = function () {
        if (xobj.readyState == 4 && xobj.status == '200') {
            // Required use of an anonymous callback as .open will NOT return a value but simply returns undefined in asynchronous mode
            callback(xobj.responseText);
        }
    };
    xobj.send(null);
}

function beginNextAction() {
    if (actions.length == 0) {
        return false;
    }
    var action = actions.shift();
    if (action.actionType == 'add') {
        var shipID = action.add.ID;
        var startingPosition = [action.add.X, action.add.Y];
        var sizeCategory = action.add.sizeCategory;
        var isEnemy = action.add.isEnemy;
        addShip(shipID, startingPosition, sizeCategory, isEnemy);
    }
    else if (action.actionType == 'kill') {
        var shipID = action.kill.ID;
        killShip(shipID);
    }
    else if (action.actionType == 'explosion') {
        var position = [action.explosion.X, action.explosion.Y];
        addExplosion(position);
    }
    else if (action.actionType == 'shoot') {
        for (s in action.shoot.shots) {
            var originPosition = [action.shoot.shots[s].oX, action.shoot.shots[s].oY];
            var targetPosition = [action.shoot.shots[s].tX, action.shoot.shots[s].tY];
            var isEnemy = action.shoot.shots[s].isEnemy;
            var isHit = action.shoot.shots[s].isHit;
            var isCrit = action.shoot.shots[s].isCrit;
            addBeamShot(originPosition, targetPosition, isEnemy, isHit, isCrit);
        }
    }
    else if (action.actionType == 'move') {
        var shipID = action.move.ID;
        var targetPosition = [action.move.X, action.move.Y];
        moveShip(shipID, targetPosition);
    }

    return true;
}



// GAME LOOP
function startGame() {
    ships = [];
    beamShots = [];
    explosions = [];
    getActions();
}
function gameLoop() {
    var now = Date.now();
    var dt = (now - lastTime) / 1000.0;

    if (keepGoing) {
        updateShips(dt);
        updateBeams(dt);
        updateExplosions(dt);
        renderAll();
    }

    lastTime = now;

    var moreActions = true;
    if (shipsDone && beamsDone && explosionsDone) {
        moreActions = beginNextAction();
    }

    if (moreActions || !(shipsDone && beamsDone && explosionsDone)) {
        requestAnimFrame(gameLoop);
    }
    else {
        renderAll();
        resetButtons();
    }

}

// Cross-browser requestAnimationFrame
var requestAnimFrame = (function () {
    return window.requestAnimationFrame ||
        window.webkitRequestAnimationFrame ||
        window.mozRequestAnimationFrame ||
        window.oRequestAnimationFrame ||
        window.msRequestAnimationFrame ||
        function (callback) {
            window.setTimeout(callback, 1000 / 60);
        };
})();



// RENDERING
function renderAll() {
    ctx.clearRect(0, 0, gameH, gameW);
    renderEntities(backgroundTiles);
    renderEntities(ships);
    renderLines(beamShots);
    renderEntities(explosions);
}



// SHIPS
function addShip(shipID, startingPosition, sizeCategory, isEnemy) {
    // TODO: add size and isEnemy
    var imgPath = 'images/ship';
    if (isEnemy) {
        imgPath = 'images/enemy';
    }

    ships.push({
        id: shipID,
        pos: [startingPosition[0] * cellSize, startingPosition[1] * cellSize],
        sprite: new Sprite(imgPath + sizeCategory + '.png', [0, 0], [cellSize, cellSize]),
        target: [startingPosition[0] * cellSize, startingPosition[1] * cellSize],
        isAlive: true,
        lastFacing: 'N',
        sizeCategory: sizeCategory,
        isEnemy: isEnemy
    });
}

function moveShip(shipID, targetPosition) {
    // find ship with matching ID
    for (var i = 0; i < ships.length; i++) {
        if (ships[i].id == shipID) {
            ships[i].target = [targetPosition[0] * cellSize, targetPosition[1] * cellSize];
        }
    }

    // notify the system that one or more ships has movement to resolve
    shipsDone = false;
}

function killShip(shipID) {
    // find ship with matching ID
    for (var i = 0; i < ships.length; i++) {
        if (ships[i].id == shipID) {
            ships[i].isAlive = false;

            addExplosion([ships[i].pos[0] / 32, ships[i].pos[1] / 32]);
        }
    }
}

function updateShips(dt) {
    // check to remove
    var toRemove = [];
    for (var i = 0; i < ships.length; i++) {
        if (ships[i].isAlive == false) {
            toRemove.push(ships[i]);
        }
    }
    for (var j = 0; j < toRemove.length; j++) {
        var index = ships.indexOf(toRemove[j]);
        if (index > -1) {
            ships.splice(index, 1);
            isDone = true;
        }
    }

    // update position
    for (var i = 0; i < ships.length; i++) {
        if (ships[i].pos[0] != ships[i].target[0] || ships[i].pos[1] != ships[i].target[1]) {
            var facing = '';

            // vertical
            if (ships[i].pos[1] > ships[i].target[1]) { // is below
                facing += 'N';
                if (ships[i].pos[1] - (shipSpeed * dt) < ships[i].target[1]) { // is within 1 frame of target
                    ships[i].pos[1] = ships[i].target[1];
                }
                else {
                    ships[i].pos[1] -= shipSpeed * dt;
                }
            }
            else if (ships[i].pos[1] < ships[i].target[1]) { // is above
                facing += 'S';
                if (ships[i].pos[1] + (shipSpeed * dt) > ships[i].target[1]) { // is within 1 frame of target
                    ships[i].pos[1] = ships[i].target[1];
                }
                else {
                    ships[i].pos[1] += shipSpeed * dt;
                }
            }

            // horizontal
            if (ships[i].pos[0] > ships[i].target[0]) { // is to the right of
                facing += 'W';
                if (ships[i].pos[0] - (shipSpeed * dt) < ships[i].target[0]) { // is within 1 frame of target
                    ships[i].pos[0] = ships[i].target[0];
                }
                else {
                    ships[i].pos[0] -= shipSpeed * dt;
                }
            }
            else if (ships[i].pos[0] < ships[i].target[0]) { // is to the left of
                facing += 'E';
                if (ships[i].pos[0] + (shipSpeed * dt) > ships[i].target[0]) { // is within 1 frame of target
                    ships[i].pos[0] = ships[i].target[0];
                }
                else {
                    ships[i].pos[0] += shipSpeed * dt;
                }
            }

            if (facing == '') {
                facing = ships[i].lastFacing;
            }
            ships[i].lastFacing = facing;
            // update sprite
            var imgPath = 'images/ship';
            if (ships[i].isEnemy) {
                imgPath = 'images/enemy';
            }
            ships[i].sprite = new Sprite(imgPath + ships[i].sizeCategory + '.png', [cellSize * dir.indexOf(facing), 0], [cellSize, cellSize])
            // complete Action
            if (ships[i].pos[0] == ships[i].target[0] && ships[i].pos[1] == ships[i].pos[1]) {
                shipsDone = true;
            }
        }

    }
}



// BEAMS
function addBeamShot(originPosition, targetPosition, isEnemy, isHit, isCrit) {


    // vary the origin & target points so the beams are slightly random
    var oX = randomizePoint(originPosition[0]);
    var oY = randomizePoint(originPosition[1]);
    var tX = randomizePoint(targetPosition[0]);
    var tY = randomizePoint(targetPosition[1]);

    // thickness changes based on hit or crit
    var thickness = beamMissThickness;
    if (isHit) {
        thickness = beamHitThickness;
    }
    if (isCrit) {
        thickness = beamCritThickness;
    }

    // color based on friendly/enemy
    var color = beamFriendlyColor;
    if (isEnemy) {
        color = beamEnemyColor;
    }

    // delay beam a random amount so that not all beams appear at once
    var delay = defaultBeamRenderDelay + rollFloat(beamRenderDelayMaxRange);
    //if (rollDice(2) == 1) {
    //    delay += rollFloat(beamRenderDelayMaxRange);
    //} else {
    //    delay -= rollFloat(beamRenderDelayMaxRange);
    //}

    // add to the array
    beamShots.push({
        lineWidth: thickness,
        strokeStyle: color,
        origin: [oX, oY],
        target: [tX, tY],
        duration: defaultBeamDuration,
        startingDuration: defaultBeamDuration,
        renderDelay: delay
    });

    // notify the system that one or more beams now exist and must be resolved
    beamsDone = false;
}

function randomizePoint(targetN) {
    var dN = targetN * cellSize + (cellSize / 2);
    if (rollDice(2) == 1) {
        dN += rollDice(rndBeamDeviation);
    } else {
        dN -= rollDice(rndBeamDeviation);
    }
    return dN;
}

function updateBeams(dt) {
    var toRemove = [];
    // update beams: if their duration is still longer than the delta-time, update; else mark to remove
    for (var i = 0; i < beamShots.length; i++) {
        if (beamShots[i].renderDelay <= 0) {
            if (beamShots[i].duration < dt) {
                toRemove.push(beamShots[i]);
            }
            else {
                var shot = beamShots[i];
                beamShots[i].duration = shot.duration - dt;
                // update alpha for fade in/out over duration
                var colorArray = shot.strokeStyle.split(',');
                var halfDuration = shot.startingDuration / 2;
                var alpha = 0.0;
                if (halfDuration <= shot.duration) { // ascending
                    // starting 300, hd = 150, duration = 250, at = 50
                    alpha = 1 - (shot.duration / shot.startingDuration);
                }
                else { // descending
                    // starting 300, hd = 150, duration = 100, at = 50
                    alpha = (shot.duration / shot.startingDuration);
                }
                var colorString = colorArray[0] + ',' + colorArray[1] + ',' + colorArray[2] + ',' + alpha.toFixed(2) + ')';
                beamShots[i].strokeStyle = colorString;
            }
        }
        else {
            beamShots[i].renderDelay = beamShots[i].renderDelay - dt;
        }
    }
    // if there are beams in the list, removing them might end the action
    maybeDone = false;
    if (beamShots.length > 0) {
        maybeDone = true;
    }

    // remove the expired beams
    for (var j = 0; j < toRemove.length; j++) {
        var index = beamShots.indexOf(toRemove[j]);
        if (index > -1) {
            beamShots.splice(index, 1);
        }
    }

    // if there are no more beams but there were before we removed them, 
    // then we just finished a firing action.
    if (maybeDone) {
        if (beamShots.length == 0) {
            beamsDone = true;
        }
    }
}



// EXPLOSIONS
function addExplosion(position) {
    // add the explosion to the array
    explosions.push({
        pos: [position[0] * cellSize, position[1] * cellSize],
        sprite: new Sprite('images/explosion.png',
                           [0, 0],
                           [32, 32],
                           16,
                           [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12],
                           null,
                           true)
    });

    // notify the system that one or more explosions now exist that must be resolved
    explosionsDone = false;
}

function updateExplosions(dt) {
    // if there are explosions in the list, removing them might end the action
    maybeDone = false;
    if (explosions.length > 0) {
        maybeDone = true;
    }

    for (var i = 0; i < explosions.length; i++) {
        explosions[i].sprite.update(dt);

        // Remove if animation is done
        if (explosions[i].sprite.done) {
            explosions.splice(i, 1);
            i--;
        }
    }

    // if there are no more explosions but there were before we removed them, 
    // then we just finished an action.
    if (maybeDone) {
        if (explosions.length == 0) {
            explosionsDone = true;
        }
    }
}
