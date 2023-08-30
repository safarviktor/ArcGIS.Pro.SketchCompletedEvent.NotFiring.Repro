# ArcGIS.Pro.SketchCompletedEvent.NotFiring.Repro

1. Pro starts
2. Select a new Map
3. Main menu -> Add-In
4. Group 1 -> find button "SketchCompletedEvent not firing demo" -> click
5. a new dock pane appears, wait a bit for a feature layer to load into the map
6. click the button in the dock pane
![image](https://github.com/safarviktor/ArcGIS.Pro.SketchCompletedEvent.NotFiring.Repro/assets/31205931/bd9f6340-5792-4ddb-bf4e-5aba8356e9c8)


Expected behaviour:
1. button click -> a feature is selected and veretex editing is started
2. upon user ends the vertex editor with "Finish", a message pops

Actual behaviour:
1. no message pops as the SketchCompletedEvent was not fired
