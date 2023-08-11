CREATE TABLE [dbo].[EllipticalBottom6533]
(
	[EllipticalBottomTypeId] INT NOT NULL, 
    [D] FLOAT NOT NULL,
    [h1] FLOAT NOT NULL,
    [h] FLOAT NOT NULL,
    [s] FLOAT NOT NULL,
    [F] FLOAT NOT NULL,
    [V] FLOAT NOT NULL,
    [m] FLOAT NOT NULL, 
    CONSTRAINT [FK_EllipticalBottom6533_ToEllipticalBottom6533Type] FOREIGN KEY ([EllipticalBottomTypeId]) REFERENCES [EllipticalBottom6533Type]([Id])
)
