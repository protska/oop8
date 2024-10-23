/*
Скрипт развертывания для D:\8\OOP8\OOP8\DATABASE1.MDF

Этот код был создан программным средством.
Изменения, внесенные в этот файл, могут привести к неверному выполнению кода и будут потеряны
в случае его повторного формирования.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "D:\8\OOP8\OOP8\DATABASE1.MDF"
:setvar DefaultFilePrefix "D_\8\OOP8\OOP8\DATABASE1.MDF_"
:setvar DefaultDataPath "C:\Users\бактерия\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB\"
:setvar DefaultLogPath "C:\Users\бактерия\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB\"

GO
:on error exit
GO
/*
Проверьте режим SQLCMD и отключите выполнение скрипта, если режим SQLCMD не поддерживается.
Чтобы повторно включить скрипт после включения режима SQLCMD выполните следующую инструкцию:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'Для успешного выполнения этого скрипта должен быть включен режим SQLCMD.';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO

IF (SELECT OBJECT_ID('tempdb..#tmpErrors')) IS NOT NULL DROP TABLE #tmpErrors
GO
CREATE TABLE #tmpErrors (Error int)
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL READ COMMITTED
GO
BEGIN TRANSACTION
GO
PRINT N'Идет создание Таблица [dbo].[AUTHORS]…';


GO
CREATE TABLE [dbo].[AUTHORS] (
    [ID]        INT            NOT NULL,
    [NAME]      NVARCHAR (50)  NULL,
    [SURNAME]   NVARCHAR (50)  NULL,
    [BIOGRAPHY] NVARCHAR (500) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

CREATE TABLE BOOKS (
	ID INT PRIMARY KEY,
	NAME NVARCHAR(50),
	AUTHOR INT,
	PICTURE_PATH NVARCHAR(1000),
	DESCRIPTION NVARCHAR(500),
	FOREIGN KEY (AUTHOR) REFERENCES AUTHORS(ID)
);

INSERT INTO AUTHORS VALUES (1, 
							'Stephen', 
							'King', 
							'Stephen Edwin King is an American author. Called the "King of Horror", he has also explored other genres, among them suspense, crime, science-fiction, fantasy and mystery.'
							);
INSERT INTO AUTHORS VALUES (2,
							'Erich Maria', 
							'Remarque',
				            'Erich Maria Remarque was a German-born novelist. His landmark novel All Quiet on the Western Front, based on his experience in the Imperial German Army during World War I, was an international bestseller which created a new literary genre of veterans writing about conflict.'
							);
INSERT INTO AUTHORS VALUES (3,
							'Joanne', 
						    'Rowling',
							'Joanne Rowling CH OBE FRSL; born 31 July 1965, known by her pen name J. K. Rowling, is a British author and philanthropist. She wrote Harry Potter, a seven-volume fantasy series published from 1997 to 2007.'
							);
INSERT INTO AUTHORS VALUES (4,
							'Agatha', 
							'Christie',
							'Dame Agatha Mary Clarissa Christie, Lady Mallowan, DBE was an English writer known for her 66 detective novels and 14 short story collections, particularly those revolving around fictional detectives Hercule Poirot and Miss Marple.'
							 );

INSERT INTO BOOKS VALUES (1, 
						'Talisman', 
						1, 
						'https://www.google.com/search?sa=X&sca_esv=6e87d5184cf45ead&sxsrf=ADLYWIK7R7DoLK9K_tXbBLPeLzDRa9-MXA:1715426942569&q=The+Talisman&stick=H4sIAAAAAAAAAONgFuLUz9U3MCy0NC9TAjPNTC2NTbSkspOt9JPy87P1E0tLMvKLrEDsYoX8vJzKRaw8IRmpCiGJOZnFuYl5O1gZAdQ3gtVGAAAA&ved=2ahUKEwimqLKsv4WGAxXmg_0HHRBEBzcQgOQBegQIMhAG&biw=1280&bih=593&dpr=1.5#&tbs=kac:1,kac_so:0', 
						'Twelve-year-old Jack Sawyer embarks on an epic quest--a walk from the seacoast of New Hampshire to the California coast--to find the talisman that will save his dying mother''s life.'
						)
INSERT INTO BOOKS VALUES (2,
						'Harry Potter And Sorcerer''s Stone',
						3,
						'https://www.google.com/imgres?q=harry%20potter%20book&imgurl=https%3A%2F%2Fm.media-amazon.com%2Fimages%2FI%2F71RVt35ZAbL._AC_UF1000%2C1000_QL80_.jpg&imgrefurl=https%3A%2F%2Fwww.amazon.com%2FHarry-Potter-Sorcerers-Stone-Rowling%2Fdp%2F0590353403&docid=O5QKhXXqXawYVM&tbnid=DnOpBvLeBSNRiM&vet=12ahUKEwjh2tfu8JyGAxUGExAIHYBQCTMQM3oECB4QAA..i&w=676&h=1000&hcb=2&ved=2ahUKEwjh2tfu8JyGAxUGExAIHYBQCTMQM3oECB4QAA',
						'The novels chronicle the lives of a young wizard, Harry Potter, and his friends Hermione Granger and Ron Weasley, all of whom are students at Hogwarts School of Witchcraft and Wizardry.'
						);
INSERT INTO BOOKS VALUES (3,
						'Harry Potter And Order Of The Phoenix',
						3,
						'https://www.google.com/imgres?q=harry%20potter%205%20book&imgurl=https%3A%2F%2Fm.media-amazon.com%2Fimages%2FI%2F71pgI2ou5oL._AC_UF1000%2C1000_QL80_.jpg&imgrefurl=https%3A%2F%2Fwww.amazon.com%2FHarry-Potter-Order-Phoenix-Book%2Fdp%2F043935806X&docid=d0hjWJDl625umM&tbnid=GIKbNcL2IGhgKM&vet=12ahUKEwi87JKV8ZyGAxWabfEDHR8hBHMQM3oECBUQAA..i&w=659&h=1000&hcb=2&ved=2ahUKEwi87JKV8ZyGAxWabfEDHR8hBHMQM3oECBUQAA',
						'The novels chronicle the lives of a young wizard, Harry Potter, and his friends Hermione Granger and Ron Weasley, all of whom are students at Hogwarts School of Witchcraft and Wizardry.'
						)
INSERT INTO BOOKS VALUES (4,
						'Death On The Nile',
						4,
						'https://www.google.com/search?sa=X&sca_esv=e11fd8c8d000e92d&biw=1280&bih=593&sxsrf=ADLYWIKF6yyv2zH_itmIbKd3OClpZFBMyQ:1715427485319&q=Poirot+sul+Nilo&stick=H4sIAAAAAAAAAONgFmLXz9U3yElJUeIEMYwKs8sMtaSyk630k_Lzs_UTS0sy8ousQOxihfy8nMpFrPwB-ZlF-SUKxaU5Cn6ZOfk7WBkBV4U_8kcAAAA&ved=2ahUKEwijzJmvwYWGAxWC3gIHHaEuPZsQgOQBegQIMxAG',
						'Death on the Nile is a work of detective fiction by British writer Agatha Christie, published in the UK by the Collins Crime Club on 1 November 1937 and in the US by Dodd, Mead and Company the following year.'
						)
SELECT * FROM BOOKS;
SELECT * FROM AUTHORS;

CREATE PROCEDURE [ADDBOOK]
    @BookID INT,
    @BookName NVARCHAR(50),
    @AuthorID INT,
    @PicturePath NVARCHAR(1000),
    @Description NVARCHAR(500)
AS
BEGIN
    INSERT INTO BOOKS (ID, NAME, AUTHOR, PICTURE_PATH, DESCRIPTION)
    VALUES (@BookID, @BookName, @AuthorID, @PicturePath, @Description);
END;

CREATE PROCEDURE [ADDAUTHOR]
    @AuthorID INT,
    @AuthorName NVARCHAR(50),
    @AuthorSurname NVARCHAR(50),
    @Biography NVARCHAR(500)
AS
BEGIN
    INSERT INTO AUTHORS(ID, NAME, SURNAME, BIOGRAPHY)
    VALUES (@AuthorID, @AuthorName, @AuthorSurname, @Biography);
END;


CREATE PROCEDURE [DELETEBOOK]
    @BookID INT
AS
BEGIN
	DELETE FROM BOOKS WHERE ID = @BOOKID;
END;

CREATE PROCEDURE [DELETEAUTHOR]
    @AuthorID INT
AS
BEGIN
	DELETE FROM AUTHORS WHERE ID = @AuthorID;
END;

CREATE PROCEDURE [UPDATEBOOK]
    @BookID INT,
    @BookName NVARCHAR(50),
    @AuthorID INT,
    @PicturePath NVARCHAR(1000),
    @Description NVARCHAR(500)
AS
BEGIN
	DELETE FROM AUTHORS, PICTURE_PATH, DESCRIPTION)
    VALUES (@BookID, @BookName, @AuthorID, @PicturePath, @Description);
END;

CREATE PROCEDURE [UPDATEAUTHOR]
    @AuthorID INT,
    @AuthorName NVARCHAR(50),
    @AuthorSurname  NVARCHAR(50),
    @Biography NVARCHAR(500)
AS
BEGIN
	DELETE FROM AUTHORS WHERE ID = @AuthorID;
	INSERT INTO AUTHORS (ID, NAME, SURNAME, BIOGRAPHY)
    VALUES (@AuthorID, @AuthorName, @AuthorSurname, @Biography);
END;


CREATE PROCEDURE [UPDATEBOOK]
    @BookID INT,
    @BookName NVARCHAR(50),
    @AuthorID INT,
    @PicturePath NVARCHAR(1000),
    @Description NVARCHAR(500)
AS
BEGIN
	DELETE FROM BOOKS WHERE ID = @BOOKID;
	INSERT INTO BOOKS (ID, NAME, AUTHOR, PICTURE_PATH, DESCRIPTION)
    VALUES (@BookID, @BookName, @AuthorID, @PicturePath, @Description);
END;


GO
IF @@ERROR <> 0
   AND @@TRANCOUNT > 0
    BEGIN
        ROLLBACK;
    END

IF OBJECT_ID(N'tempdb..#tmpErrors') IS NULL
    CREATE TABLE [#tmpErrors] (
        Error INT
    );

IF @@TRANCOUNT = 0
    BEGIN
        INSERT  INTO #tmpErrors (Error)
        VALUES                 (1);
        BEGIN TRANSACTION;
    END


GO

IF EXISTS (SELECT * FROM #tmpErrors) ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT>0 BEGIN
PRINT N'Транзакции обновления базы данных успешно завершены.'
COMMIT TRANSACTION
END
ELSE PRINT N'Сбой транзакций обновления базы данных.'
GO
IF (SELECT OBJECT_ID('tempdb..#tmpErrors')) IS NOT NULL DROP TABLE #tmpErrors
GO
GO
PRINT N'Обновление завершено.';


GO
