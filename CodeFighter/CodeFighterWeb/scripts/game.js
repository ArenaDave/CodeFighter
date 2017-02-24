/// <reference path="scripts/render.js"/>
/// <reference path="scripts/utility.js"/>
/// <reference path="scripts/sprite.js"/>
/// <reference path="scripts/resources.js"/>

// globals
var canvas;
var ctx;

// magic numbers for height and width of canvas
var gameH = 800;
var gameW = 800;

// magic number for width of each 'cell' of the grid.
var cellSize = 32;

// facing directions for sprites
var dir = ['W', 'NW', 'N', 'NE', 'E', 'SE', 'S', 'SW'];

// collections of things to render
var backgroundTiles = [];
var ships = [];
var beamShots = [];
var torpedoShots = [];
var explosions = [];

// magic number for ship animation speed
var shipSpeed = 32;

// magic numbers for beam weapon animations
var rndBeamDeviation = 10;
var defaultBeamDuration = 300;
var defaultBeamRenderDelay = 300;
var beamRenderDelayMax = 600;


$(document).ready(function () {
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
        'images/ship2.png'
    ]);
    resources.onReady(init);
});




// INIT
function init() {
    initBackground();
    initShips();
    renderAll();
    gameLoop();
}
function initBackground() {
    var index = 0;
    for (var x = 0; x < gameW / cellSize; x++) {
        for (var y = 0; y < gameH / cellSize; y++) {
            var rand = rollDice(4);
            var name = 'images/Galaxy_star_field' + rand + '.png';
            backgroundTiles[index] = {
                pos: [x * cellSize, y * cellSize],
                sprite: new Sprite(name, [0, 0], [cellSize, cellSize])
            }
            index++;
        }
    }
}
function initShips() {
    // TODO: get ships from service	
    ships[0] = {
        name: 'first',
        pos: [5 * cellSize, 5 * cellSize],
        sprite: new Sprite('images/ship2.png', [0, 0], [24, 24]),
        target: [6 * cellSize, 6 * cellSize],
        isAlive: true

    };
    ships[1] = {
        name: 'second',
        pos: [20 * cellSize, 20 * cellSize],
        sprite: new Sprite('images/ship2.png', [0, 0], [24, 24]),
        target: [19 * cellSize, 19 * cellSize],
        isAlive: true
    };

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


var lastTime = Date.now();

// GAME LOOP
function gameLoop() {
    var now = Date.now();
    var dt = (now - lastTime) / 1000.0;

    updateShips(dt);
    updateBeams(dt);
    updateTorpedoes(dt);
    updateExplosions(dt);
    renderAll();

    lastTime = now;
    requestAnimFrame(gameLoop);
    // next turn...how?

}



// RENDERING
function renderAll() {
    renderEntities(backgroundTiles);
    renderEntities(ships);
    renderLines(beamShots);
    renderEntities(torpedoShots);
    renderEntities(explosions);
}



// SHIPS
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
        }
    }

    // update position
    for (var i = 0; i < ships.length; i++) {
        if (ships[i].pos != ships[i].target) {
            var facing = '';

            // vertical
            if (ships[i].pos[0] > ships[i].target[0]) { // is below
                facing += 'N';
                if (ships[i].pos[0] - (shipSpeed * dt) < ships[i].target[0]) { // is within 1 frame of target
                    ships[i].pos[0] = ships[i].target[0];
                }
                else {
                    ships[i].pos[0] -= shipSpeed * dt;
                }
            }
            else if (ships[i].pos[0] < ships[i].target[0]) { // is above
                facing += 'S';
                if (ships[i].pos[0] + (shipSpeed * dt) > ships[i].target[0]) { // is within 1 frame of target
                    ships[i].pos[0] = ships[i].target[0];
                }
                else {
                    ships[i].pos[0] += shipSpeed * dt;
                }
            }

            // horizontal
            if (ships[i].pos[1] > ships[i].target[1]) { // is to the right of
                facing += 'W';
                if (ships[i].pos[1] - (shipSpeed * dt) < ships[i].target[1]) { // is within 1 frame of target
                    ships[i].pos[1] = ships[i].target[1];
                }
                else {
                    ships[i].pos[1] -= shipSpeed * dt;
                }
            }
            else if (ships[i].pos[1] < ships[i].target[1]) { // is to the left of
                facing += 'E';
                if (ships[i].pos[1] + (shipSpeed * dt) > ships[i].target[1]) { // is within 1 frame of target
                    ships[i].pos[1] = ships[i].target[1];
                }
                else {
                    ships[i].pos[1] += shipSpeed * dt;
                }
            }
            if (facing == '') {
                facing = 'N';
            }
            // update sprite
            ships[i].sprite = new Sprite('images/ship2.png', [24 * dir.indexOf(getDirection(ships[i].pos, ships[i].target)), 0], [24, 24])
        }

    }
}



// BEAMS
function addBeamShot(origin, target, color, isHit, isCrit) {


    // vary the origin & target points so the beams are slightly random
    var oX = origin[0] * cellSize + rollDice(rndBeamDeviation);
    var oY = origin[1] * cellSize + rollDice(rndBeamDeviation);
    var tX = target[0] * cellSize + rollDice(rndBeamDeviation);
    var tY = target[1] * cellSize + rollDice(rndBeamDeviation);

    // thickness changes based on hit or crit
    var thickness = 1;
    if (isHit) {
        thickness = 2;
    }
    if (isCrit) {
        thickness = 3;
    }

    // delay beam a random amount so that not all beams appear at once
    var delay = defaultBeamRenderDelay + rollDice(beamRenderDelayMax - defaultBeamRenderDelay);

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
                var colorArray = shot.color.split(',');
                var halfDuration = shot.startingDuration / 2;
                var alpha = 0.0;
                if (halfDuration <= shot.duration) { // ascending
                    // starting 300, hd = 150, duration = 250, at = 50
                    alpha = 1 - ((shot.startingDuration - shot.duration) / halfDuration);
                }
                else { // descending
                    // starting 300, hd = 150, duration = 100, at = 50
                    alpha = (halfDuration - shot.duration) / halfDuration;
                }
                var colorString = colorArray[0] + ',' + colorArray[1] + ',' + colorArray[2] + ',' + alpha.toFixed(2) + ')';
                beamShots[i].color = colorString;
            }
        }
        else {
            beamShots[i].renderDelay = beamShots[i].renderDelay - dt;
        }
    }
    // remove the expired beams
    for (var j = 0; j < toRemove.length; j++) {
        var index = beamShots.indexOf(toRemove[j]);
        if (index > -1) {
            beamShots.splice(index, 1);
        }
    }
}


// TORPEDOES
function updateTorpedoes(dt) {

}


// EXPLOSIONS
function updateExplosions(dt) {

}




