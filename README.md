4 лаба.
Реализация службы DataManager.

Служба Service1 была создана в предыдущей лабораторной.

Программа начинается в классе Program. Она обращается к файлам xml и json и берет информацию: строки подключения к базам данных ApplicationInsights и AdventureWorks2019 и путь к SourceDirectory и TargetDirectory.

Далее создаётся объект класса DBApplicationInsights, который отвечает за логирование.

Таблица выглядит следующим образом:

![alt text](https://github.com/polinasevko/labs-CSharp/blob/main/lr4/Pics/1.png)

Хранимые процедуры AddAction и ClearAction выглядят следующим образом:

![alt text](https://github.com/polinasevko/labs-CSharp/blob/main/lr4/Pics/2.png)

![alt text](https://github.com/polinasevko/labs-CSharp/blob/main/lr4/Pics/3.png)

Далее все действия предаставляются DataManager. Он подключается к базе AdventureWorks2019 с помощью класса BDAdventure и формирует xml-документ. Метод GetPerson возвращает записи из БД.

Хранимые процедуры:

![alt text](https://github.com/polinasevko/labs-CSharp/blob/main/lr4/Pics/4.png)

![alt text](https://github.com/polinasevko/labs-CSharp/blob/main/lr4/Pics/5.png)

Файлы хранятся в специальной папке Person. Далее они пересылаются в папку SourceDir, где всю работу на себя берет Service1.

P.S.
![alt text](https://github.com/polinasevko/labs-CSharp/blob/main/lr4/Pics/-uv_tS8vy1c.jpg)
![alt text](https://github.com/polinasevko/labs-CSharp/blob/main/lr4/Pics/9Qu6nOQFsoo.jpg)
![alt text](https://github.com/polinasevko/labs-CSharp/blob/main/lr4/Pics/iBaByUOUHKo.jpg)
