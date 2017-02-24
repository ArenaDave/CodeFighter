

// RENDERING
function renderEntities(list) {
    for (var i = 0; i < list.length; i++) {
        renderEntity(list[i]);
    }
}

function renderEntity(entity) {
    ctx.save();
    // translate to position the renderer
    ctx.translate(entity.pos[0], entity.pos[1]);
    entity.sprite.render(ctx);
    ctx.restore();
}

function renderLines(list){
	for(var i = 0; i < list.length; i++){
		renderLine(list[i]);
	}
}

function renderLine(line){
	if(line.renderDelay<=0)
	{
		ctx.save();
		ctx.lineWidth = line.lineWidth;
		ctx.strokeStyle = line.strokeStyle;
		ctx.beginPath();
		ctx.moveTo(line.origin[0],line.origin[1]);
		ctx.lineTo(line.target[0],line.target[1]);
		ctx.stroke();
		ctx.restore();
	}
}
