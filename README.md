# Mauzilla Despair

## About
Game for the Global Game Jam 2020, developed in Bremen (see https://globalgamejam.org/2020/games/mauzilla-despair-9)

## Description
*Mauzilla Despair* is a Co-op strategy game where one player plays against the three others.

One player plays as Mauzilla! His job is to destroy the city. The 3 other players are artisans (craftsman) who can rebuild the buildings that Mauzilla destroys.

## How To Play
This game requires four players!

You can use Joysticks or a keyboard (or both).


## Credits
### Team

 - [Andreas](http://github.com/brean) – Hornorary Mayor (Programming + 2.5D tileset, Cat Lazer Sound)
 - Lena – Weird Cat Lady (Character Art)
 - Luka – Project Destruction Manager (Programming)
 - Malte – Lazer Expert (Programming, Particles)
 - Kim – Home Improvement Expert (Programming)
 - Filiz – Running Picture Senior Expert (Animation, Sound)

### External Assets
 - Isometric Buildings: https://kenney.nl/assets/isometric-buildings
 - Isometric City: https://kenney.nl/assets/isometric-city

## Tools Used
* Unity Game Engine (v2019.3.0f6)
* GIMP




## Development Notes

### Ansteuerung der Controller Eingaben

Player 1 - 4 sind konfiguriert.

Links / Rechts. Auf der Xbox Controller ist der linke Analogstick.

Links = -1
Rechts = 1
```bash
float direction = Input.getAxis("Player1JoyHorizontal");
```
Das gleiche für vertical:
```bash
float direction = Input.getAxis("Player1JoyVertical");
```


Buttons:

SuperNintendo / Xbox / Names
```
    <X>     |     <Y>     |       Up       
<Y>     <A> | <X>     <B> | Left      Right
    <B>     |     <A>     |      Down      
```

Die Buttons über:
Normal = False
Gedrückt = True
```bash
bool pressed = Input.getButton("Player1ButtonUp");
```
