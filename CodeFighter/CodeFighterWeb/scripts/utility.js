
// SHIP FACING
// based on 0,0 in top-left corner, with ascending values when going right or down.
function getDirection(origin,target){
	var result = '';
	// up
	if(origin[0] > target[0]) { 
		result = result + 'N';
	}
	else if(origin[0] < target[0]) {
		result = result + 'S';
	}
	if(origin[1] < target[1]){
		result = result + 'E';
	}
	else if(origin[1] > target[1]) {
		result = result + 'W';
	}
	if (result == '') {
	    result = 'N';
	}
	return result;
}


// RANDOMIZER
function rollDice(sides){
	return Math.floor(Math.random() * sides + 1);
}