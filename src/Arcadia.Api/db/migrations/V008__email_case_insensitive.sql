--Before this version, emails were unique and case sensitive. This script fixes that.
CREATE UNIQUE INDEX players_email_lower_uq ON players (lower(email));
ALTER TABLE players DROP CONSTRAINT players_email_uq;