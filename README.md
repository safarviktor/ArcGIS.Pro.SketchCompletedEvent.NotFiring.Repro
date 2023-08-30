# ArcGIS.Pro.SketchCompletedEvent.NotFiring.Repro

1. Pro starts
2. Select a new Map
3. Main menu -> Add-In
4. Group 1 -> find button "SketchCompletedEvent not firing demo" -> click
5. a new dock pane appears, wait a bit for a feature layer to load into the map
6. click the button in the dock pane
![image](https://github.com/safarviktor/ArcGIS.Pro.SketchCompletedEvent.NotFiring.Repro/assets/31205931/a5f06115-5271-45ec-8f0a-d3f0d5f0d89c)


Expected behaviour:
1. button click -> a feature is selected and veretex editing is started
2. upon user ends the vertex editor with "Finish", a message pops

Actual behaviour:
1. no message pops as the SketchCompletedEvent was not fired
