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

var dir = ['W', 'NW', 'N', 'NE', 'E', 'SE', 'S', 'SW'];

var player = {
    pos: [5, 5],
    facing: 'NE',
    sprite: null

};


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

function init() {
    // draw background as randomized starfield
    for (var x = 0; x < gameW / 32; x++) {
        for (var y = 0; y < gameH / 32; y++) {
            var rand = Math.floor((Math.random() * 4) + 1);
            var img = resources.get('images/Galaxy_star_field' + rand + '.png');
            var thisX = x * 32;
            var thisY = y * 32;
            ctx.drawImage(img, thisX, thisY);

        }
    }

    // set the player sprite based on the facing
    player.sprite = new Sprite('images/ship2.png', [24 * dir.indexOf(player.facing), 0], [24, 24])

    // render the player
    renderEntity(player);

}

function renderEntities(list) {
    for (var i = 0; i < list.length; i++) {
        renderEntity(list[i]);
    }
}

function renderEntity(entity) {
    ctx.save();
    // entity.pos is grid coordinate, so multiply by the cellsize to get the pixel coordinates
    // add half of cell size minus sprite size to position in the center of the grid coordinate
    var x = entity.pos[0] * cellSize + ((cellSize - entity.sprite.size[0]) / 2);
    var y = entity.pos[1] * cellSize + ((cellSize - entity.sprite.size[1]) / 2);
    // translate to position the renderer
    ctx.translate(x, y);
    entity.sprite.render(ctx);
    ctx.restore();
}

