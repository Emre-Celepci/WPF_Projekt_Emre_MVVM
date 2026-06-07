-- Datenbank ProjektEmre

--create database ProjektEmre;
--go

use ProjektEmre;
go

-- DDL Kommandos

drop table if exists Highscores;

drop table if exists Field;
drop table if exists FieldEvent;

drop table if exists Player;
drop table if exists Score;
drop table if exists Figur;



go

create table Figur ( 
	FigurID   integer		primary key
,	FigurName varchar(255) 
,	ImageName varchar(255)
);
go

create table Score ( 
	ScoreValue   integer	primary key
);
go


create table Field ( 
	FieldID   integer		primary key
,	FieldName varchar(255)
,	FieldTyp  varchar(255)
,	ImageName varchar(255)
);
go

create table Player ( 
	PlayerID    integer identity(1,1)		primary key
,	PlayerName  varchar(255)
,	Figur       varchar(255) 
,	Score	    integer
);
go

create table Highscores ( 
	PlayerID   integer		unique references Player
,	PlayerName varchar(255)
,	Score	   integer
);
go


--DML Kommandos

begin transaction;
go

insert into Score
  (ScoreValue )
values
  (5)
, (10)
, (20)
, (50)
, (100)
, (200)
, (1000)
;
go

insert into Figur
  (FigurID , FigurName  , ImageName)
values
  (1      , 'Archer' ,'Archer')
, (2      , 'Dragon' , 'Dragon')
, (3      , 'Knight' , 'Knight')
, (4      , 'Mage'   , 'Mage')
;

insert into Field
  (FieldID , FieldName   , FieldTyp , ImageName        )  
values
  (1      , 'Start'      , 'Event' , 'StartField'      )
, (2      , 'Normal'     , 'Normal', 'NormalField'     )
, (3      , 'DiceAgain'  , 'Event' , 'DiceAgainField'  )
, (4      , 'ScorePlus'  , 'Event' , 'ScorePLusField'  )
, (5      , 'ScoreMinus' , 'Event' , 'ScoreMinusField' )
, (6      , 'RoundSetOut', 'Event' , 'RoundSetOutField')
;




insert into Player
 ( PlayerName, Figur, Score)
 Values
 ( 'KingCasper', 'Dragon', 999999)
;
go

insert into Highscores
 ( PlayerID,PlayerName, Score)
 Values
 ( 1, 'KingCasper', 999999)
;
go

commit;
go


-- DQL Kommandos 


select * 
  from Field
;
select * 
  from Figur
;
select * 
  from Score
;
go


select * 
  from Player
;
go

select * 
  from Highscores
;
go

