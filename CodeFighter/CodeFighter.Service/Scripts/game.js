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
var cannonShots = [];
var torpedoShots = [];
var explosions = [];
var features = [];

// magic number for ship movement speed
var shipSpeed = 64;

// magic numbers for beam weapon animations
var rndBeamDeviation = 10;
var defaultBeamDuration = 0.7;
var beamMissThickness = 1;
var beamHitThickness = 2;
var beamCritThickness = 3;
var beamFriendlyColor = 'rgba(255,0,0,0.0)';
var beamEnemyColor = 'rgba(0,255,0,0.0)';
var cannonFriendlyColor = 'rgba(255,0,0,1.0)';
var cannonEnemyColor = 'rgba(0,255,0,1.0)';

// colors for messages
var friendlyMoveMessageColor = '#2A506C';
var friendlyCannonMessageColor = '#3F607A';
var friendlyBeamMessageColor = '#587489';
var friendlyTorpedoMessageColor = '#173F5D';
var enemyMoveMessageColor = '#4B2E72';
var enemyCannonMessageColor = '#735E90';
var enemyBeamMessageColor = '#5E4481';
var enemyTorpedoMessageColor = '#391A62';


// flags to determine if the next action should be pulled
var shipsDone = true;
var beamsDone = true;
var cannonsDone = true;
var torpedoesDone = true;
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
        'images/explosion.png',
        'images/asteroid1.png',
        'images/asteroid2.png',
        'images/asteroid3.png',
        'images/asteroid4.png',
        'images/asteroid5.png',
        'images/asteroid6.png'
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
    startButton.className += " hidden";
    // show the pause button
    var pauseButton = document.getElementById("btnPause");
    pauseButton.className = pauseButton.className.replace(/(?:^|\s) ?hidden(?!\S)/g, '');
    // show the loader
    var loader = document.getElementById("loader");
    loader.className = loader.className.replace(/(?:^|\s) ?hidden(?!\S)/g, '');
}
function pause() {
    // pause the game (rendering keeps going, need to fix this to actually pause the entire loop for performance)
    keepGoing = false;
    // hide the pause button
    var pauseButton = document.getElementById("btnPause");
    pauseButton.className += " hidden";
    // show the resume button
    var resumeButton = document.getElementById("btnResume");
    resumeButton.className = resumeButton.className.replace(/(?:^|\s) ?hidden(?!\S)/g, '');
}
function resume() {
    // reset the last time (or else it will skip a lot of frames) and resume the loop
    lastTime = Date.now();
    keepGoing = true;
    // hide the resume button
    var resumeButton = document.getElementById("btnResume");
    resumeButton.className += " hidden";
    // show the pause button
    var pauseButton = document.getElementById("btnPause");
    pauseButton.className = pauseButton.className.replace(/(?:^|\s) ?hidden(?!\S)/g, '');
}
function resetButtons() {
    // hide the resume button
    if (!document.getElementById("btnResume").className.match(/(?:^|\s) ?hidden(?!\S)/)) {
        var resumeButton = document.getElementById("btnResume");
        resumeButton.className += " hidden";
    }
    // hide the pause button
    if (!document.getElementById("btnPause").className.match(/(?:^|\s) ?hidden(?!\S)/)) {
        var pauseButton = document.getElementById("btnPause");
        pauseButton.className += " hidden";
    }
    // show the start button
    if (document.getElementById("btnStart").className.match(/(?:^|\s) ?hidden(?!\S)/)) {
        var startButton = document.getElementById("btnStart");
        startButton.className = startButton.className.replace(/(?:^|\s) ?hidden(?!\S)/g, '');
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
        scenarioID: '508E62F3-5B0E-4716-8B8D-FBCA000317A2',
        playerID: '550D672D-F40A-4A13-9212-DEB4CFE27F2D',
        playerCode: 'dostuff'
    };
    // prep request for POST
    var http = new XMLHttpRequest();
    var url = 'http://localhost/CodeFighter.Service/api/gameEngine';
    http.open('POST', url, true);
    // when done, parse result into actions and start game loop
    // TODO: add error handling for error results
    http.onreadystatechange = function () {
        if (http.readyState == 4 && http.status == 200) {
            // hide the loader
            var loader = document.getElementById("loader");
            loader.className += " hidden";
            actions = JSON.parse(http.responseText).Data;
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
    if (action.actionType == 'message') {
        var messages = action.messages;
        addMessages(messages);
    }
    else if (action.actionType == 'add') {
        var shipID = action.details.id;
        var startingPosition = [action.details.x, action.details.y];
        var sizeCategory = action.details.sizeCategory;
        var isEnemy = action.details.isEnemy;
        addShip(shipID, startingPosition, sizeCategory, isEnemy);
        var messages = action.messages;
        addMessages(messages);
    }
    else if (action.actionType == 'kill') {
        var shipID = action.details.id;
        killShip(shipID);
        var messages = action.messages;
        addMessages(messages);
    }
    else if (action.actionType == 'explosion') {
        var position = [action.details.x, action.details.y];
        addExplosion(position);
        var messages = action.messages;
        addMessages(messages);
    }
    else if (action.actionType == 'shoot') {
        for (s in action.details.shots) {
            if (ships.filter(function (obj) { return obj.id == action.details.shots[s].target; }).length > 0
                && ships.filter(function (obj) { return obj.id == action.details.shots[s].origin; }).length > 0) {
                // get origin ship position
                var test = ships.filter(function (obj) { return obj.id == action.details.shots[s].origin; });
                var originShip = ships.filter(function (obj) { return obj.id == action.details.shots[s].origin; })[0];
                var originPosition = originShip.pos;

                // get target ship position
                var targetPosition = ships.filter(function (obj) { return obj.id == action.details.shots[s].target; })[0].pos;

                // check if origin is enemy ship
                var isEnemy = originShip.isEnemy;

                var isHit = action.details.shots[s].isHit;
                var isCrit = action.details.shots[s].isCrit;
                if (action.details.shots[s].firingType == 'Beam') {
                    addBeamShot(originPosition, targetPosition, isEnemy, isHit, isCrit);
                }
                else if (action.details.shots[s].firingType == 'Cannon') {
                    addCannonShot(originPosition, targetPosition, isEnemy, isHit, isCrit);
                }
                else if (action.details.shots[s].firingType == 'Torpedo') {
                    addTorpedoShot(originPosition, targetPosition, isEnemy, isHit, isCrit);
                }
            }
            else {
                if (ships.filter(function (obj) { return obj.id == action.details.shots[s].target; }).length <= 0) {
                    alert('Attempting to fire at invalid target: ' + action.details.shots[s].target);
                }
                if (ships.filter(function (obj) { return obj.id == action.details.shots[s].origin; }).length <= 0) {
                    alert('Attempting to fire from invalid origin: ' + action.details.shots[s].origin);
                }
            }
        }
        var messages = action.messages;
        var color = null;
        if (originShip.isEnemy) {
            if (action.details.shots[0].firingType == 'Beam') {
                color = enemyBeamMessageColor;
            }
            else if (action.details.shots[0].firingType = 'Cannon') {
                color = enemyCannonMessageColor;
            }
            else {
                color = enemyTorpedoMessageColor;
            }
        }
        else {
            if (action.details.shots[0].firingType == 'Beam') {
                color = friendlyBeamMessageColor;
            }
            else if (action.details.shots[0].firingType = 'Cannon') {
                color = friendlyCannonMessageColor;
            }
            else {
                color = friendlyTorpedoMessageColor;
            }
        }
        addMessages(messages, color);
    }
    else if (action.actionType == 'move') {
        var shipID = action.details.id;
        if (ships.filter(function (obj) { return obj.id == shipID }).length <= 0) {
            alert('Attempting to move invalid ship: ' + shipID);
        }
        var targetPosition = [action.details.x, action.details.y];
        moveShip(shipID, targetPosition);
        var messages = action.messages;
        var shipcandidates = ships.filter(function (obj) { return obj.id == shipID });
        var color = null;
        if (shipcandidates.length > 0) {
            if (shipcandidates[0].isEnemy) {
                color = enemyMoveMessageColor;
            }
            else {
                color = friendlyMoveMessageColor;
            }
        }
        addMessages(messages, color);
    }
    else if (action.actionType == 'ship') {
        var shipInfo = action.details;
        var sd = document.getElementById("shipDetail" + shipInfo.id);
        if (sd != null) {
            updateShipDetail(shipInfo);
        }
        else {
            addShipDetail(shipInfo);
        }
    }
    else if (action.actionType == 'newround') {
        var roundCounter = document.getElementById('roundCounter');
        roundCounter.innerHTML = action.messages[0];
    }
    else if (action.actionType == 'feature') {
        addFeature(action.details);
    }
    // keep the game loop going
    return true;
}

function addMessages(messages, color) {
    if (messages != null) {
        var div = document.createElement('div');
        div.className = 'innerMessage panel';
        var prepend = '';
        var postpend = '';
        if (color != null) {
            prepend = "<span style='color:" + color + "'>";
            postpend = "</span>";
        }
        for (var i = 0; i < messages.length; i++) {
            div.innerHTML += prepend + messages[i] + postpend;
            if (i < messages.length - 1)
                div.innerHTML += '<br/>';
        }
        var messageBox = document.getElementById('messageBox');
        messageBox.appendChild(div);
        messageBox.scrollTop = messageBox.scrollHeight;
    }
}

// GAME LOOP
function startGame() {
    // clear everything and get the actions list
    ships = [];
    beamShots = [];
    explosions = [];
    var enemyShips = document.getElementById("enemyShipList").getElementsByTagName("div");
    for (var i = 0; i < enemyShips.length; i++) {
        enemyShips[i].outerHTML = "";
    }
    var playerShips = document.getElementById("playerShipList").getElementsByTagName("div");
    for (var i = 0; i < playerShips.length; i++) {
        playerShips[i].outerHTML = "";
    }
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
        updateCannons(dt);
        updateTorpedoes(dt);
        updateExplosions(dt);
        updateFeatures(dt);
        renderAll();
    }
    // store last execution time
    lastTime = now;

    // if we're done animating actions, get the next action
    var moreActions = true;
    if (shipsDone && beamsDone && cannonsDone && torpedoesDone && explosionsDone) {
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
    renderEntities(features);
    renderProjectiles(cannonShots, 3);
    renderProjectiles(torpedoShots);
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
function addShipDetail(shipInfo) {
    var reference = document.getElementById("shipTemplate");
    var div = document.createElement("div");
    div.id = "shipDetail" + shipInfo.id;
    div.className = "shipTemplate col-xs-12 col-sm-6";
    div.innerHTML = reference.innerHTML;
    if (shipInfo.ownerIsAI) {
        document.getElementById("enemyShipList").appendChild(div);
    }
    else {
        document.getElementById("playerShipList").appendChild(div);
    }
    updateShipDetail(shipInfo);
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
function removeShipDetail(shipInfo) {
    document.getElementById("shipDetail" + shipInfo.id).outerHTML = "";
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
function updateShipDetail(shipInfo) {
    var sd = document.getElementById("shipDetail" + shipInfo.id);
    if (sd != null) {
        var shipName = sd.getElementsByClassName("shipName")[0];
        var shipSize = sd.getElementsByClassName("shipSize")[0];
        var className = sd.getElementsByClassName("className")[0];
        var shipHp = sd.getElementsByClassName("shipHp")[0];
        var shipPos = sd.getElementsByClassName("shipPos")[0];
        var shipParts = sd.getElementsByClassName("shipParts")[0];

        if (shipInfo.isDestroyed) {
            shipName.innerHTML = "<s>" + shipInfo.name + "</s>";
        }
        else {
            shipName.innerHTML = shipInfo.name;
        }
        shipSize.innerHTML = shipInfo.sizeName;
        className.innerHTML = shipInfo.className;
        shipHp.innerHTML = shipInfo.hp;
        shipPos.innerHTML = shipInfo.pos;
        shipParts.innerHTML = "";
        for (var i = 0; i < shipInfo.parts.length; i++) {
            shipParts.innerHTML += "<div>" + shipInfo.parts[i] + "</div>";
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

    // add to the array
    beamShots.push({
        lineWidth: thickness,
        strokeStyle: color,
        origin: [oX, oY],
        target: [tX, tY],
        duration: 0,
        totalDuration: defaultBeamDuration
    });

    // notify the system that one or more beams now exist and must be resolved
    beamsDone = false;
}
function randomizePoint(targetN) {
    // reusable
    // starting from the center of the cell, add or subtract a random deviation
    var dN = targetN + (cellSize / 2);
    if (rollDice(2) == 1) {
        dN += rollDice(rndBeamDeviation);
    } else {
        dN -= rollDice(rndBeamDeviation);
    }
    return dN;
}
function easeInQuint(t) {
    return Math.pow(t, 5);
}
function easeOutQuint(t) {
    return 1 - easeInQuint(1 - t);
}
function easeInOutQuint(t) {
    if (t < 0.5) return easeInQuint(t * 2.0) / 2.0;
    return 1 - easeInQuint((1 - t) * 2.0) / 2.0;
}
function updateBeams(dt) {
    var toRemove = [];
    // update beams: if their duration is still longer than the delta-time, update; else mark to remove
    for (var i = 0; i < beamShots.length; i++) {

        if (beamShots[i].duration + dt > beamShots[i].totalDuration) {
            toRemove.push(beamShots[i]);
        }
        else {
            var shot = beamShots[i];
            beamShots[i].duration = shot.duration + dt;
            // update alpha for fade in/out over duration
            var colorArray = shot.strokeStyle.split(',');
            var halfDuration = shot.totalDuration / 2;
            var alpha = 0.0;
            if (shot.duration < halfDuration) {
                alpha = easeInOutQuint(shot.duration / halfDuration);
            }
            else {
                alpha = 1 - easeInOutQuint((shot.duration - halfDuration) / halfDuration);
            }
            var colorString = colorArray[0] + ',' + colorArray[1] + ',' + colorArray[2] + ',' + alpha.toFixed(2) + ')';
            beamShots[i].strokeStyle = colorString;
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

// CANNONS
function addCannonShot(originPosition, targetPosition, isEnemy, isHit, isCrit) {
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
    var color = cannonFriendlyColor;
    if (isEnemy) {
        color = cannonEnemyColor;
    }

    // get the vertices
    var vertices = [];
    vertices.push({ x: oX, y: oY });
    vertices.push({ x: tX, y: tY });
    var points = calcWaypoints(vertices);

    // add to the array
    cannonShots.push({
        lineWidth: thickness + beamMissThickness * 2,
        strokeStyle: color,
        t: 1,
        points: points
    });

    // notify the system that one or more cannons now exist and must be resolved
    cannonsDone = false;

}
function calcWaypoints(vertices) {
    var waypoints = [];
    for (var i = 1; i < vertices.length; i++) {
        var pt0 = vertices[i - 1];
        var pt1 = vertices[i];
        var dx = pt1.x - pt0.x;
        var dy = pt1.y - pt0.y;
        for (var j = 0; j < 32; j++) {
            var x = pt0.x + dx * j / 32;
            var y = pt0.y + dy * j / 32;
            waypoints.push({
                x: x,
                y: y
            });
        }
    }
    return (waypoints);
}
function updateCannons(dt) {
    var toRemove = [];
    // update cannons, if t > points.length+8 (last bullet), remove; else increase t
    for (var i = 0; i < cannonShots.length; i++) {
        //var cannon = cannonShots[i];
        if (cannonShots[i].t >= cannonShots[i].points.length + 8) {
            toRemove.push(cannonShots[i]);
        }
        else {
            cannonShots[i].t = cannonShots[i].t + 48 * dt;
        }

    }
    // if there are cannons in the list, removing them might end the action
    maybeDone = false;
    if (cannonShots.length > 0) {
        maybeDone = true;
    }

    // remove expired cannons
    for (var j = 0; j < toRemove.length; j++) {
        var index = cannonShots.indexOf(toRemove[j]);
        if (index > -1) {
            cannonShots.splice(index, 1);
        }
    }

    // if there are no more cannons but there were before we removed them, 
    // then we just finished a firing action.
    if (maybeDone) {
        if (cannonShots.length == 0) {
            cannonsDone = true;
        }
    }
}

// TORPEDOES
function addTorpedoShot(originPosition, targetPosition, isEnemy, isHit, isCrit) {
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
    var color = cannonFriendlyColor;
    if (isEnemy) {
        color = cannonEnemyColor;
    }

    // get the vertices
    var vertices = [];
    vertices.push({ x: oX, y: oY });
    vertices.push({ x: tX, y: tY });
    var points = calcWaypoints(vertices);

    // add to the array
    torpedoShots.push({
        lineWidth: thickness + beamMissThickness * 2,
        pulseLineWidth: thickness*2 + beamMissThickness * 2,
        pulseCount: 0,
        strokeStyle: color,
        t: 1,
        points: points
    });

    // notify the system that one or more cannons now exist and must be resolved
    torpedoesDone = false;
}
function updateTorpedoes(dt) {
    var toRemove = [];
    // update torpedoes, if t > points.length, remove; else increase t
    for (var i = 0; i < torpedoShots.length; i++) {
        if (torpedoShots[i].t >= torpedoShots[i].points.length) {
            toRemove.push(torpedoShots[i]);
        }
        else {
            torpedoShots[i].t = torpedoShots[i].t + 32 * dt;
            if (torpedoShots[i].pulseCount++ % 4 == 0) {
                var pulse = torpedoShots[i].pulseLineWidth;
                torpedoShots[i].pulseLineWidth = torpedoShots[i].lineWidth;
                torpedoShots[i].lineWidth = pulse;
            }
        }

    }

    // if there are torpedoes in the list, removing them might end the action
    maybeDone = false;
    if (torpedoShots.length > 0) {
        maybeDone = true;
    }

    // remove expired torpedoes
    for (var j = 0; j < toRemove.length; j++) {
        var index = torpedoShots.indexOf(toRemove[j]);
        if (index > -1) {
            torpedoShots.splice(index, 1);
        }
    }

    // if there are no more torpedoes but there were before we removed them, 
    // then we just finished a firing action.
    if (maybeDone) {
        if (torpedoShots.length == 0) {
            torpedoesDone = true;
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


// FEATURES
function addFeature(featureDetails) {
    if (featureDetails.type == 'asteroid') {
        var rand = rollDice(4);
        var imageName = 'images/asteroid' + rand + '.png';
        var randomAnimationStart = rollDice(16);
        var sequence = [];//[0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15]
        for (var i = 0; i < 16; i++) {
            sequence.push((randomAnimationStart + i) % 16);
        }
        var randomSpeed = rollDice(10);

        features.push({
            pos: [featureDetails.position.X * cellSize, featureDetails.position.Y * cellSize],
            sprite: new Sprite(imageName,
                [0, 0],
                [32, 32],
                randomSpeed,
                sequence,
                null,
                false)
        });
    }
}
function updateFeatures(dt) {
    for (var i = 0; i < features.length; i++) {
        features[i].sprite.update(dt);
    }
}