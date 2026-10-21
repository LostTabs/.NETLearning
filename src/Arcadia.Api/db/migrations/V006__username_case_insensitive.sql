--Before this version, usernames were unique, but not case sensitive. This script fixes that.
CREATE UNIQUE INDEX players_username_lower_uq ON players (lower(username));
ALTER TABLE players DROP CONSTRAINT players_username_uq;