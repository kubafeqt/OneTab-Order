CREATE TABLE [dbo].[Samples] (
		-- Vlastní ID pro tento řádek (doporučeno pro správu dat)
    [SampleId]     INT           IDENTITY (1, 1) NOT NULL, 
    [RecognitionId]         INT           NOT NULL, -- Cizí klíč ukazující na ImageRecognition
    [SampleHash]        VARCHAR (64)  NOT NULL,
     -- Primární klíč této tabulky
    CONSTRAINT [PK_Samples] PRIMARY KEY CLUSTERED ([SampleId] ASC),
    
    CONSTRAINT [UQ_Samples_Recognition_Sample]
        UNIQUE ([RecognitionId], [SampleHash]),

    -- správná vazba
    CONSTRAINT [FK_Samples_ImageRecognition] FOREIGN KEY ([RecognitionId]) 
        REFERENCES [dbo].[ImageRecognition] ([RecognitionId])
        ON DELETE CASCADE -- když smažete ImageRecognition, smažou se i jeho samples data
);