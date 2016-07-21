-- Script Date: 20.07.2016 13:45  - ErikEJ.SqlCeScripting version 3.5.2.58
CREATE TABLE [YoutubeSong] (
  [songid] nvarchar(100) NOT NULL
, [playlistnr] int NOT NULL
, [playlistid] nchar(200) NOT NULL
, [title] nchar(100)
, CONSTRAINT [sqlite_autoindex_YoutubeSong_1] PRIMARY KEY ([songid],[playlistnr],[playlistid])
);
