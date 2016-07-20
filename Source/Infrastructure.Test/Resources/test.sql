-- Script Date: 20.07.2016 12:17  - ErikEJ.SqlCeScripting version 3.5.2.58
-- Database information:
-- Database: C:\Users\sns.B19\Documents\webplayer\Source\webplayer.db
-- ServerVersion: 3.9.2
-- DatabaseSize: 7 KB
-- Created: 05.07.2016 23:34

-- User Table information:
-- Number of tables: 3
-- playlist: -1 row(s)
-- song: -1 row(s)
-- YoutubeSong: -1 row(s)

SELECT 1;
PRAGMA foreign_keys=OFF;
BEGIN TRANSACTION;
CREATE TABLE [playlist] (
  [Id] nchar(200) NOT NULL
, [name] nchar(100) NULL
, CONSTRAINT [sqlite_autoindex_playlist_1] PRIMARY KEY ([Id])
);
COMMIT;

