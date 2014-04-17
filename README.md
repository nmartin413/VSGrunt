VSGrunt
=======

Grunt integration for Visual Studio 2012 & 2013

+ Runs npm install on save of the package.json file (in the root)
+ Adds Grunt commands to the Project right-click menu
+ You can configure custom triggers with the `VSGrunt.xml` file
+ pattern: regex to match against the file path
+ gruntTask: the name of the task you want to run. Run multiple tasks by separating with a space.

Must have the grunt CLI installed

`npm install -g grunt-cli`



=======

Sample VSGrunt.xml

	<?xml version="1.0" encoding="utf-8" ?>
	<VSGrunt>
	  <triggers>
	    <trigger pattern="\.html" gruntTask="default" />
	    <trigger pattern="\.js" gruntTask="default" />
	  </triggers>
	  <Settings />
	</VSGrunt>