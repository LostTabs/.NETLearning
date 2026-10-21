-- Setup: a valid player to test against (this one should succeed)
INSERT INTO players (username, email, country) VALUES ('Diogo', 'diogo@mail.com', 'PT');
-- Setup: items that use COIN as a price
INSERT INTO items (sku, display_name, price_amount, price_currency_code, stackable) VALUES ('COIN_001', 'itemA', 99, 'COIN', true);

-- 1. A balance of -100
INSERT INTO player_balances (player_id, currency_code, balance) VALUES ((SELECT id FROM players WHERE username = 'diogo'), 'GEM', -100);
-- ERROR:  new row for relation "player_balances" violates check constraint "player_balances_balance_ck"
-- DETAIL:  Failing row contains (1, GEM, -100, 2026-10-21 10:57:32.047765+00).

-- 2. An inventory row for player ID 99999, which doesn't exist.
INSERT INTO player_balances (player_id, currency_code, balance) VALUES (99999, 'COIN', 100);
-- ERROR:  insert or update on table "player_balances" violates foreign key constraint "player_balances_player_fk"
-- DETAIL:  Key (player_id)=(99999) is not present in table "players".

-- 3. Deleting the COIN currency while items are priced in it.
DELETE FROM currencies WHERE code = 'COIN';
-- ERROR:  update or delete on table "currencies" violates RESTRICT setting of foreign key constraint "items_price_currency_fk" on table "items"
-- DETAIL:  Key (code)=(COIN) is referenced from table "items". 

-- 4. Two players with the same email.
INSERT INTO players (username, email, country) VALUES ('Filipe', 'diogo@mail.com', 'UK');
-- ERROR:  duplicate key value violates unique constraint "players_email_uq"
-- DETAIL:  Key (email)=(diogo@mail.com) already exists.

-- 5. A player called Diogo, then another called diogo.
INSERT INTO players (username, email, country) VALUES ('Diogo', 'f@mail.com', 'PT');
-- ERROR:  duplicate key value violates unique constraint "players_username_lower_uq"
-- DETAIL:  Key (lower(username))=(diogo) already exists.