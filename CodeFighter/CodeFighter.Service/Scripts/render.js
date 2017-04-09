

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

function renderLines(list) {
    for (var i = 0; i < list.length; i++) {
        renderLine(list[i]);
    }
}

function renderLine(line) {
    ctx.save();
    ctx.lineWidth = line.lineWidth;
    ctx.strokeStyle = line.strokeStyle;
    ctx.beginPath();
    ctx.moveTo(line.origin[0], line.origin[1]);
    ctx.lineTo(line.target[0], line.target[1]);
    ctx.stroke();
    ctx.restore();

}


function renderProjectiles(list, totalProjectiles) {
    for (var i = 0; i < list.length; i++) {
        renderProjectile(list[i], totalProjectiles);
    }
}

function renderProjectile(projectile, totalProjectiles) {
    if (projectile.t < projectile.points.length) {
        ctx.save();
        ctx.lineWidth = projectile.lineWidth;
        ctx.strokeStyle = projectile.strokeStyle;
        ctx.lineCap = "round";
        ctx.beginPath();
        var points = projectile.points;
        var t = Math.round(projectile.t);

        if (t >= 1 && t < points.length) {
            ctx.moveTo(points[t - 1].x, points[t - 1].y);
            ctx.lineTo(points[t].x, points[t].y);
        }

        if (totalProjectiles != null) {
            for (var i = 0; i < totalProjectiles; i++) {
                if (t > (1 + i * 4) && t < points.length + i * 4) {
                    ctx.moveTo(points[t - (1 + i * 4)].x, points[t - (1 + i * 4)].y);
                    ctx.lineTo(points[t - (i * 4)].x, points[t - (i * 4)].y);
                }
            }
        }

        ctx.stroke();
        ctx.restore();
    }
}