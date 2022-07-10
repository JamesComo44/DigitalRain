# DigitalRain
![Class Diagram](http://www.plantuml.com/plantuml/proxy?cache=no&src=https://raw.githubusercontent.com/JamesComo44/DigitalRain/master/class_diagram.puml)

## Ethan's To Do List
1. Refactor StandardRaindropFactory and PerGridCoordinateRaindropFactory to stay DRY.
   The different behaviors should be combinable.  You should be able to create multiple
   combinations easily in code.  If it's easy to do in code, it'll be easy to do in config.
2. Refactor to Options pattern using .NET configuration lib.
3. Refactor to Ninject (or other) DI container.  Can you rewrite combinations from (1) as
   config files now?
4. Feature: Type out a phrase and hit enter -> that phrase appears on the screen like
   "HELLO WORLD!" does in your prototype. Hit escape -> the phrase "melts away" as the
   raindrops resume their descent. Rinse and repeat.