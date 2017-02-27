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
    // begin the game
    keepGoing = true;
    startGame();
    // hide the start button
    var startButton = document.getElementById("btnStart");
    startButton.className += "hidden";
    // show the pause button
    var pauseButton = document.getElementById("btnPause");
    pauseButton.className = pauseButton.className.replace(/(?:^|\s)hidden(?!\S)/g, '');
}
function pause() {
    // pause the game (rendering keeps going, need to fix this to actually pause the entire loop for performance)
    keepGoing = false;
    // hide the pause button
    var pauseButton = document.getElementById("btnPause");
    pauseButton.className += "hidden";
    // show the resume button
    var resumeButton = document.getElementById("btnResume");
    resumeButton.className = resumeButton.className.replace(/(?:^|\s)hidden(?!\S)/g, '');
}
function resume() {
    // reset the last time (or else it will skip a lot of frames) and resume the loop
    lastTime = Date.now();
    keepGoing = true;
    // hide the resume button
    var resumeButton = document.getElementById("btnResume");
    resumeButton.className += "hidden";
    // show the pause button
    var pauseButton = document.getElementById("btnPause");
    pauseButton.className = pauseButton.className.replace(/(?:^|\s)hidden(?!\S)/g, '');
}
function resetButtons() {
    // hide the resume button
    if (!document.getElementById("btnResume").className.match(/(?:^|\s)hidden(?!\S)/)) {
        var resumeButton = document.getElementById("btnResume");
        resumeButton.className += "hidden";
    }
    // hide the pause button
    if (!document.getElementById("btnPause").className.match(/(?:^|\s)hidden(?!\S)/)) {
        var pauseButton = document.getElementById("btnPause");
        pauseButton.className += "hidden";
    }
    // show the start button
    if (document.getElementById("btnStart").className.match(/(?:^|\s)hidden(?!\S)/)) {
        var startButton = document.getElementById("btnStart");
        startButton.className = startButton.className.replace(/(?:^|\s)hidden(?!\S)/g, '');
    }
    // stop the loop
    keepGoing = false;
}

// RANDOMIZER
// re-seed Math.random each time for better distribution
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
    // build payload to send
    var data = {
        scenarioID: '00000000-0000-0000-0000-000000000000',
        playerID: '00000000-0000-0000-0000-000000000000',
        playerCode: 'dostuff'
    };
    // prep request for POST
    var http = new XMLHttpRequest();
    var url = 'http://codefighter.local/api/gameEngine';
    http.open('POST', url, true);
    // when done, parse result into actions and start game loop
    // TODO: add error handling for error results
    http.onreadystatechange = function () {
        if (http.readyState == 4 && http.status == 200) {
            actions = JSON.parse(http.responseText);
            gameLoop();
        }
    };
    http.setRequestHeader('Content-Type', 'application/json; charset=UTF-8');
    // send payload
    http.send(JSON.stringify(data));

}

function beginNextAction() {
    // check if we're out of actions (ends the game loop)
    if (actions.length == 0) {
        return false;
    }
    // grab action at index == 0 (and remove from array)
    var action = actions.shift();
    if (action.actionType == 'add') {
        var shipID = action.details.id;
        var startingPosition = [action.details.x, action.details.y];
        var sizeCategory = action.details.sizeCategory;
        var isEnemy = action.details.isEnemy;
        addShip(shipID, startingPosition, sizeCategory, isEnemy);
    }
    else if (action.actionType == 'kill') {
        var shipID = action.details.id;
        killShip(shipID);
    }
    else if (action.actionType == 'explosion') {
        var position = [action.details.x, action.details.y];
        addExplosion(position);
    }
    else if (action.actionType == 'shoot') {
        for (s in action.details.shots) {
            var originPosition = [action.details.shots[s].oX, action.details.shots[s].oY];
            var targetPosition = [action.details.shots[s].tX, action.details.shots[s].tY];
            var isEnemy = action.details.shots[s].isEnemy;
            var isHit = action.details.shots[s].isHit;
            var isCrit = action.details.shots[s].isCrit;
            addBeamShot(originPosition, targetPosition, isEnemy, isHit, isCrit);
        }
    }
    else if (action.actionType == 'move') {
        var shipID = action.details.id;
        var targetPosition = [action.details.x, action.details.y];
        moveShip(shipID, targetPosition);
    }
    // keep the game loop going
    return true;
}



// GAME LOOP
function startGame() {
    // clear everything and get the actions list
    ships = [];
    beamShots = [];
    explosions = [];
    getActions();
}
function gameLoop() {
    // calculate delta since last execution (to smooth regardless of even framerates)
    var now = Date.now();
    var dt = (now - lastTime) / 1000.0;

    // for right now the loop keeps going but the screen isnt updated.
    // eventually the pause/resume buttons should pause or trigger the whole game loop.
    if (keepGoing) {
        updateShips(dt);
        updateBeams(dt);
        updateExplosions(dt);
        renderAll();
    }
    // store last execution time
    lastTime = now;

    // if we're done animating actions, get the next action
    var moreActions = true;
    if (shipsDone && beamsDone && explosionsDone) {
        moreActions = beginNextAction();
    }

    // if there are more actions, or we're not done animating the current one(s), keep looping
    if (moreActions || !(shipsDone && beamsDone && explosionsDone)) {
        requestAnimFrame(gameLoop);
    }
    else {
        // final render and reset the buttons
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
    // clear all and re-render because of the lines
    ctx.clearRect(0, 0, gameH, gameW);
    renderEntities(backgroundTiles);
    renderEntities(ships);
    renderLines(beamShots);
    renderEntities(explosions);
}



// SHIPS
function addShip(shipID, startingPosition, sizeCategory, isEnemy) {
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
    // reusable
    // starting from the center of the cell, add or subtract a random deviation
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
                    alpha = 1 - (shot.duration / shot.startingDuration);
                }
                else { // descending
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
