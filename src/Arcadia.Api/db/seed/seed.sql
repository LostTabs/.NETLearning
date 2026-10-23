INSERT INTO items(sku, display_name, price_amount, price_currency_code, stackable) VALUES ('ITEM001', 'item1', 100, 'COIN', true);
INSERT INTO items(sku, display_name, price_amount, price_currency_code, stackable) VALUES ('ITEM002', 'item2', 50, 'COIN', true);
INSERT INTO items(sku, display_name, price_amount, price_currency_code, stackable) VALUES ('ITEM003', 'item3', 100, 'GEM', true);
INSERT INTO items(sku, display_name, price_amount, price_currency_code, stackable) VALUES ('ITEM004', 'item4', 200, 'GEM', true);
INSERT INTO items(sku, display_name, price_amount, price_currency_code, stackable) VALUES ('ITEM005', 'NO_ONE_HAS', 100, 'COIN', true);

INSERT INTO players (username, email, country) VALUES ('NO_INVENTORY', 'NO_INVENTORY@mail.com', 'PT');
INSERT INTO players (username, email, country) VALUES ('NO_BALANCE', 'NO_BALANCE@mail.com', 'PT');
INSERT INTO players (username, email, country) VALUES ('NO_SCORE', 'NO_SCORE@mail.com', 'PT');
INSERT INTO players (username, email, country) VALUES ('ONLY_COINS', 'ONLY_COINS@mail.com', 'UK');
INSERT INTO players (username, email, country) VALUES ('Player005', 'Player005@mail.com', 'UK');
INSERT INTO players (username, email, country) VALUES ('Player006', 'Player006@mail.com', 'UK');
INSERT INTO players (username, email, country) VALUES ('Player007', 'Player007@mail.com', 'US');
INSERT INTO players (username, email, country) VALUES ('Player008', 'Player008@mail.com', 'US');
INSERT INTO players (username, email, country) VALUES ('Player009', 'Player009@mail.com', 'GR');
INSERT INTO players (username, email, country) VALUES ('Player019', 'Player019@mail.com', 'GR');

INSERT INTO player_balances (player_id, currency_code, balance) VALUES ((SELECT id FROM players WHERE username = 'NO_INVENTORY'), 'COIN', 100);
INSERT INTO player_balances (player_id, currency_code, balance) VALUES ((SELECT id FROM players WHERE username = 'NO_INVENTORY'), 'GEM', 100);
INSERT INTO player_balances (player_id, currency_code, balance) VALUES ((SELECT id FROM players WHERE username = 'NO_SCORE'), 'COIN', 200);
INSERT INTO player_balances (player_id, currency_code, balance) VALUES ((SELECT id FROM players WHERE username = 'NO_SCORE'), 'GEM', 20);
INSERT INTO player_balances (player_id, currency_code, balance) VALUES ((SELECT id FROM players WHERE username = 'ONLY_COINS'), 'COIN', 50);
INSERT INTO player_balances (player_id, currency_code, balance) VALUES ((SELECT id FROM players WHERE username = 'PLAYER_005'), 'COIN', 20);
INSERT INTO player_balances (player_id, currency_code, balance) VALUES ((SELECT id FROM players WHERE username = 'PLAYER_005'), 'GEM', 20);
INSERT INTO player_balances (player_id, currency_code, balance) VALUES ((SELECT id FROM players WHERE username = 'PLAYER_006'), 'COIN', 100);
INSERT INTO player_balances (player_id, currency_code, balance) VALUES ((SELECT id FROM players WHERE username = 'PLAYER_006'), 'GEM', 50);
INSERT INTO player_balances (player_id, currency_code, balance) VALUES ((SELECT id FROM players WHERE username = 'PLAYER_007'), 'COIN', 50);
INSERT INTO player_balances (player_id, currency_code, balance) VALUES ((SELECT id FROM players WHERE username = 'PLAYER_007'), 'GEM', 10);
INSERT INTO player_balances (player_id, currency_code, balance) VALUES ((SELECT id FROM players WHERE username = 'PLAYER_008'), 'COIN', 70);
INSERT INTO player_balances (player_id, currency_code, balance) VALUES ((SELECT id FROM players WHERE username = 'PLAYER_008'), 'GEM', 27);
INSERT INTO player_balances (player_id, currency_code, balance) VALUES ((SELECT id FROM players WHERE username = 'PLAYER_009'), 'COIN', 210);
INSERT INTO player_balances (player_id, currency_code, balance) VALUES ((SELECT id FROM players WHERE username = 'PLAYER_009'), 'GEM', 210);
INSERT INTO player_balances (player_id, currency_code, balance) VALUES ((SELECT id FROM players WHERE username = 'PLAYER_010'), 'COIN', 10);
INSERT INTO player_balances (player_id, currency_code, balance) VALUES ((SELECT id FROM players WHERE username = 'PLAYER_010'), 'GEM', 0);

INSERT INTO inventory (player_id, item_id, quantity) VALUES ((SELECT id FROM players WHERE username = 'NO_BALANCE'), (SELECT item_id FROM items WHERE sku = 'ITEM002'), 10);
INSERT INTO inventory (player_id, item_id, quantity) VALUES ((SELECT id FROM players WHERE username = 'NO_BALANCE'), (SELECT item_id FROM items WHERE sku = 'ITEM003'), 10);
INSERT INTO inventory (player_id, item_id, quantity) VALUES ((SELECT id FROM players WHERE username = 'NO_SCORE'), (SELECT item_id FROM items WHERE sku = 'ITEM001'), 10);
INSERT INTO inventory (player_id, item_id, quantity) VALUES ((SELECT id FROM players WHERE username = 'NO_SCORE'), (SELECT item_id FROM items WHERE sku = 'ITEM003'), 10);
INSERT INTO inventory (player_id, item_id, quantity) VALUES ((SELECT id FROM players WHERE username = 'ONLY_COINS'), (SELECT item_id FROM items WHERE sku = 'ITEM002'), 10);
INSERT INTO inventory (player_id, item_id, quantity) VALUES ((SELECT id FROM players WHERE username = 'PLAYER_005'), (SELECT item_id FROM items WHERE sku = 'ITEM003'), 10);
INSERT INTO inventory (player_id, item_id, quantity) VALUES ((SELECT id FROM players WHERE username = 'PLAYER_005'), (SELECT item_id FROM items WHERE sku = 'ITEM004'), 10);
INSERT INTO inventory (player_id, item_id, quantity) VALUES ((SELECT id FROM players WHERE username = 'PLAYER_006'), (SELECT item_id FROM items WHERE sku = 'ITEM001'), 10);
INSERT INTO inventory (player_id, item_id, quantity) VALUES ((SELECT id FROM players WHERE username = 'PLAYER_006'), (SELECT item_id FROM items WHERE sku = 'ITEM003'), 10);
INSERT INTO inventory (player_id, item_id, quantity) VALUES ((SELECT id FROM players WHERE username = 'PLAYER_007'), (SELECT item_id FROM items WHERE sku = 'ITEM002'), 10);
INSERT INTO inventory (player_id, item_id, quantity) VALUES ((SELECT id FROM players WHERE username = 'PLAYER_007'), (SELECT item_id FROM items WHERE sku = 'ITEM004'), 10);
INSERT INTO inventory (player_id, item_id, quantity) VALUES ((SELECT id FROM players WHERE username = 'PLAYER_008'), (SELECT item_id FROM items WHERE sku = 'ITEM001'), 10);
INSERT INTO inventory (player_id, item_id, quantity) VALUES ((SELECT id FROM players WHERE username = 'PLAYER_008'), (SELECT item_id FROM items WHERE sku = 'ITEM002'), 10);
INSERT INTO inventory (player_id, item_id, quantity) VALUES ((SELECT id FROM players WHERE username = 'PLAYER_009'), (SELECT item_id FROM items WHERE sku = 'ITEM004'), 10);
INSERT INTO inventory (player_id, item_id, quantity) VALUES ((SELECT id FROM players WHERE username = 'PLAYER_009'), (SELECT item_id FROM items WHERE sku = 'ITEM003'), 10);
INSERT INTO inventory (player_id, item_id, quantity) VALUES ((SELECT id FROM players WHERE username = 'PLAYER_010'), (SELECT item_id FROM items WHERE sku = 'ITEM001'), 10);
INSERT INTO inventory (player_id, item_id, quantity) VALUES ((SELECT id FROM players WHERE username = 'PLAYER_010'), (SELECT item_id FROM items WHERE sku = 'ITEM004'), 10);

INSERT INTO leaderboard (player_id, score) VALUES ((SELECT id FROM players WHERE username = 'NO_INVENTORY'), 100);
INSERT INTO leaderboard (player_id, score) VALUES ((SELECT id FROM players WHERE username = 'NO_BALANCE'), 80);
INSERT INTO leaderboard (player_id, score) VALUES ((SELECT id FROM players WHERE username = 'ONLY_COINS'), 20);
INSERT INTO leaderboard (player_id, score) VALUES ((SELECT id FROM players WHERE username = 'PLAYER_005'), 20);
INSERT INTO leaderboard (player_id, score) VALUES ((SELECT id FROM players WHERE username = 'PLAYER_006'), 19);
INSERT INTO leaderboard (player_id, score) VALUES ((SELECT id FROM players WHERE username = 'PLAYER_007'), 90);
INSERT INTO leaderboard (player_id, score) VALUES ((SELECT id FROM players WHERE username = 'PLAYER_008'), 75);
INSERT INTO leaderboard (player_id, score) VALUES ((SELECT id FROM players WHERE username = 'PLAYER_009'), 32);
INSERT INTO leaderboard (player_id, score) VALUES ((SELECT id FROM players WHERE username = 'PLAYER_010'), 44);


SELECT setseed(0.42);  -- makes random() repeatable, so results don't change between runs

INSERT INTO players (username, email, country)
SELECT 'bot_' || g, 'bot_' || g || '@mail.com', (ARRAY['GB','PT','IE','US'])[1 + g % 4]
FROM generate_series(1, 200) AS g;

--Adding scores to bots
INSERT INTO leaderboard (player_id, score)
SELECT p.id, floor(random() * 100000)::bigint
FROM players p 
WHERE p.username LIKE 'bot\_%' AND random() < 0.9;
    
--Adding currencies to bots
INSERT INTO player_balances (player_id, currency_code, balance)
SELECT p.id, c.code, floor(random() * 10000)::bigint
FROM players p CROSS JOIN currencies c
WHERE p.username LIKE 'bot\_%' AND random() < 0.7;
    
--Adding inventory to bots
INSERT INTO inventory (player_id, item_id, quantity)
SELECT p.id, i.id, 1 + floor(random() * 10)::bigint
FROM players p CROSS JOIN items i
WHERE p.username LIKE 'bot\_%' AND i.sku <> 'ITEM005' AND random() < 0.7;