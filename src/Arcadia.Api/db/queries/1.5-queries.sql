-- JOINS
-- 1. Every player's username alongside the display name and quantity of each item they own.
-- Every player *except* the player with username NO_INVENTORY should appear.
SELECT p.username, it.display_name, inv.quantity
FROM inventory inv INNER JOIN players p ON inv.player_id = p.id 
                    INNER JOIN items it ON inv.item_id = it.id
-- Uncomment this like if you want to see only the manually inserted players
-- WHERE p.username NOT LIKE 'bot\_%'
ORDER BY p.username, it.display_name;

-- 2. Every player and the number of distinct items they own, including players with 0.
-- Every player appears; NO_INVENTORY and any bots without inventory show 0;
-- No need to use unique because inventory key is (player.id, item.id) so there can't be 2 rows
-- for the same item owned by the same player.
SELECT p.username, COUNT(inv.quantity) AS item_count
FROM players p LEFT JOIN inventory inv ON p.id = inv.player_id
GROUP BY p.id
ORDER BY p.id;

-- 3. Players who own no items. Write it twice: once with LEFT JOIN + IS NULL and once with NOT EXISTS.
-- Every player that has no items
-- V1
SELECT p.username
FROM players p LEFT JOIN inventory inv ON p.id = inv.player_id
WHERE inv.item_id IS NULL
ORDER BY p.username;
    
-- V2
SELECT p.username
FROM players p
WHERE NOT EXISTS (
    SELECT 1 
    FROM inventory inv
    WHERE inv.player_id = p.id)
ORDER BY p.username;
    
-- PROVE THEY ARE EQUAL (should run in both directions to make sure
(SELECT p.username
 FROM players p LEFT JOIN inventory inv on p.id = inv.player_id
 WHERE inv.item_id IS NULL
 ORDER BY p.username) 
EXCEPT
(SELECT p.username
 FROM players p
 WHERE NOT EXISTS (
     SELECT 1
     FROM inventory inv
     WHERE inv.player_id = p.id)
 ORDER BY p.username);

-- Breaking it
-- The internal query returns all players with some inventory + 1 line with NULL player_id for the item
-- That no one owns. The NOT IN then compares the player id of the players without inventory with all the ids 
-- of players with an inventory + null. id <> null returns unknown, and the WHERE discards it.
SELECT p.username
FROM players p 
WHERE p.id NOT IN (SELECT inv.player_id
                   FROM items i
                            LEFT JOIN inventory inv on inv.item_id = i.id)
ORDER BY p.username;

-- 4. Items that no player owns.
-- Shows item with display_name "NO_ONE_HAS", as its an item that no player has in their inventory
SELECT i.sku, i.display_name
FROM items i
WHERE NOT EXISTS (SELECT 1 FROM inventory inv WHERE inv.item_id = i.id)
ORDER BY i.sku;
    
-- 5. Every player's COIN balance, showing 0 for players with no COIN row. Write the broken version first, then the fixed one.
-- V1: broken because the LEFT JOIN adds balances to every player. The ON and WHERE do not filter anything, so it
-- shows a list of every players.
SELECT p.username
FROM players p LEFT JOIN player_balances b ON p.id = b.player_id
WHERE b.currency_code = 'COIN';

-- V2: FIXED. Now we get the balance of all players and 0 for players with no entry for 'COIN' in player_balances
SELECT p.username, COALESCE(b.balance, 0) AS coin_balance
FROM players p LEFT JOIN player_balances b ON b.player_id = p.id 
                                                    AND b.currency_code = 'COIN'
ORDER BY p.username;


--GROUP BY/HAVING
-- 6. The number of players per country, largest first.
-- Expected result:  50 bots per country (g % 4) + hand-written players
-- PT-53; US-52; GB-50; IE-50; UK-3; GR-2
SELECT p.country, COUNT(*) as player_count
FROM players p
GROUP BY p.country
ORDER BY player_count DESC, p.country;
    

-- 7. Countries with more than 50 players.
-- Expected Result: PT US
SELECT p.country, COUNT(*) AS player_count
FROM players p
GROUP BY p.country
HAVING COUNT(*) > 50
ORDER BY player_count DESC, p.country;
    
    
-- 8. Each player's total inventory value per currency (quantity × price, grouped by player and currency).
-- Player with no inventory do not appear in this result as their inventory is empty so they have nothing to report
SELECT inv.player_id, i.price_currency_code, SUM(inv.quantity * i.price_amount) as amount
FROM inventory inv INNER JOIN items i ON inv.item_id = i.id
GROUP BY inv.player_id, i.price_currency_code
ORDER BY inv.player_id, i.price_currency_code;
    
    
-- 9. Items owned by at least 3 distinct players, with the owner count.
-- Expected result: 4 (158), 1 (146), 2 (142), 3 (135) - So every item except 5 since it has no owner.
SELECT it.id, it.sku, COUNT(*) AS owners_count
FROM inventory inv
JOIN items it ON it.id = inv.item_id
GROUP BY it.id
HAVING COUNT(*) >= 3
ORDER BY owners_count DESC, it.id;

-- SUBQUERIES
-- 10. Players whose GEM balance is above the average GEM balance.
-- EXPECTED RESULT: Players with a gem balance above 4753.5. The average is counted as >= 0.
-- Only bots are above average. bot_1 8422, bot_10 7090,...
SELECT p.username, b.balance AS gem_balance 
FROM players p INNER JOIN player_balances b ON b.player_id = p.id
                                            AND b.currency_code = 'GEM'
WHERE b.balance > (
    SELECT AVG(balance)
    FROM player_balances as pb
    WHERE pb.balance >= 0 AND pb.currency_code = 'GEM'                                      
    )
ORDER BY p.username;
    
-- 11. Players who own at least one item priced in a premium currency. Use currencies.is_premium, not a hard-coded 'GEM'.

    
    
-- CTEs
-- 12. Players whose COIN-denominated inventory value is above their own country's average.

-- WINDOW FUNCTIONS
-- 13. The global leaderboard with ROW_NUMBER, RANK and DENSE_RANK side by side. Find your tied players in the output.
-- 14. Each player's rank within their country.
-- 15. "Players around me": given a player id, return that player plus the 5 above and 5 below by global rank, and include a column showing