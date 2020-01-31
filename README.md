# ggj2020
Global Game Jam 2020 Game

## Ansteuerung der Controller Eingaben

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

SuperNintendo / Xbox / Benennung
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
