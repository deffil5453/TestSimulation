# TestSimulation
Что реализовано?
1. Дроны собирают кристаллы, и относят их на базу, затем опять ищут.
2. Дроны не сталкиваются между собой, а смещаются друг от друга.

Архитектура и компоненты.
1. Есть два рабочих скрипта, для спавна кристаллов:"CrystalSpawner" и ИИ дрона "DroneAI"

Описание логики.
1. В скрипте "CrystalSpawner" кристаллы спавнятся, мы берем префаб кристалла и создаём ещё в разных позициях.
1. В скрипте "DroneAI" реализовано основное поведение для ИИ, есть методы для поиска ресурсов, сбор ресурсов и выгрузка ресурсов на базу, а также система избежания

Использованные инструменты и подходы.
1. Unity:
    NavMesh для навигации ИИ
2. IDE Visual Studio 2022  
