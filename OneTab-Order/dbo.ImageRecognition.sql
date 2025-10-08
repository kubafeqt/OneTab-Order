CREATE TABLE [dbo].[ImageRecognition] (
    [Id]                INT           IDENTITY (1, 1) NOT NULL,
    [BrowserName]       VARCHAR (128) NOT NULL,
    [SampleHash]        VARCHAR (64)  NOT NULL,
    [ScreenStartX]      INT           NOT NULL,
    [ScreenStartY]      INT           NOT NULL,
    [RecognitionStartX] INT           NULL,
    [RecognitionStartY] INT           NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

