var grunt = require('grunt');
require('./gruntfile')(grunt);

var out = [];

var allTasks = grunt.task._tasks;

for (taskName in grunt.task._tasks) {
	var task = allTasks[taskName];
	out.push({
		name: task.name,
		info: task.info
	});
}

console.log(JSON.stringify(out));