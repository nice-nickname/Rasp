# Модели
## Простые справочники
Справочники, хранящие данные, используемые эти значения во многих других таблицах

## 1. Институт (Факультет)
Справочник институтов (факультетов)
``` sql

Id          INT PRIMARY KEY IDENTITY
Name        VARCHAR

```

## 2. Кафедра
Справочник по кафедрам
``` sql

Id          INT PRIMARY KEY IDENTITY 
Name        VARCHAR
FacultyId   INT REFERENCES Институт (факультет) 

```

## 3. Корпус
Справочник по корпусам
``` sql

Id          INT PRIMARY KEY IDENTITY
Name        VARCHAR

```

## 4. Формат расписания
Список времени пар (Начало-Конец)
``` sql

Id          INT PRIMARY KEY IDENTITY
Start       TIME 
END         TIME
Order       TINYINT
FacultyId   INT REFERENCES Институт

```

## 5. Типы аудторий
Справочник типов типов аудиторий
``` sql

Id          INT PRIMARY KEY IDENTITY
Name        VARCHAR

```



---
## Базовые сущности
## 6. Аудитория
```sql

Id          INT Первыичный ключ
Code        VARCHAR
BuildingId  INT NULL REFERENCES Корпус
DepartmentId INT NULL REFERENCES Кафедра
Kinds       INT NULL REFERENCES Типы аудиторий

```

## 7. Преподаватель
Список преподавателей
``` sql

Id          INT PRIMARY KEY IDENTITY 
Name        VARCHAR
DepartmentId INT NULL REFERENCES Кафедра

```

## 8. Группа
Список групп

``` sql

Id          INT PRIMARY KEY
Code        VARCHAR 
DepartmentId INT REFERENCES Кафедра

```

## 9. Дисциплина
Список общих дисциплин
``` sql

Id          INT PRIMARY KEY
Name        VARCHAR
Type        'Зачет' | 'Дифференцированный зачет' | 'Экзамен'
DepartmentId INT NULL REFERENCES Кафедра
GroupsId INT REFERENCES Группы

```

## 10. Под-дисциплина
Под-дисциплина является реализацией конкретной дисциплины, с конкретным типом, преподавателями и количеством часов по ней
``` sql

Id          INT PRIMARY KEY
Type        'Лекция' | 'Практика' | 'Лабораторная работа' | 'Консультация' | 'Экзамен'
Hours       SMALL INT 
Teachers    INT REFERENCES Преподаватель
GroupsId    INT REFERENCES Группы

```

## 11. Занятие
Занятием является отображением под-дисциплин на конкретную аудиторию, пару и день
``` sql

Id              INT PRIMARY KEY
AuditoiumId     INT REFERENCES Аудитория
SubDisciplineId INT REFERENCES Под-дисциплина
ScheduleTableId INT REFERENCES Формат расписания 
GroupId         INT REFERENCES Группы
Day             DATE
```



---
## Сущности-связи 1:N и N:N

## 12. Аудитории к типам аудиторий
Связь многие ко многим между аудиториями и типами аудиторий
``` sql

Id              INT PRIMARY KEY IDENTITY
AccessoryId     INT REFERENCES Типы аудиторий
AuditoriumId    INT REFERENCES Аудитория

```

## 13. Преподаватели к под-дисциплинам
Связь многие-ко-многим между преподавателями и дисциплинами, которые они ведут
```sql

Id              INT PRIMARY KEY IDENTITY
TeacherId       INT REFERENCES Преподаватель
SubDisciplineId iNT REFERENCES Под-дисциплина

```

## 14. Требования к типам аудиторий для дисциплин
Связь многие-ко-многим между типом аудитории и под-дисциплиной отображает требование на наличие определенного типа аудитории при проведении занятия
```sql

Id              INT PRIMARY KEY IDENTITY
AccessoryId     INT REFERENCES Типы аудиторий
SubDisciplineId iNT REFERENCES Под-дисциплина

```

## 15. Группы к дисциплинам
У дисциплины есть список преподавателей, которую они ведут
```sql

Id              INT PRIMARY KEY IDENTITY
DisciplineId    INT REFERENCES Дисциплина
TeacherId       INT REFERENCES Преподаватель

```

## 16. Группы к под-дисциплинам
У под-дисциплин есть свой отдельный список преподавателей
```sql

Id              INT PRIMARY KEY IDENTITY
SubDisciplineId INT REFERENCES Под-дисциплина
TeacherId       INT REFERENCES Преподаватель

```


---
## Сущности, отвечающие за хранение предпочтений

## 15. Предпочтения преподавателй (?)

## 16. Предпочтения по дисциплинам (?)

## 17. Забронированные аудитории в расписании (?)