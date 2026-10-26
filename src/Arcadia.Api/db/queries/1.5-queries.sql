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
ORDER BY p.username;

-- 3. Players who own no items. Write it twice: once with LEFT JOIN + IS NULL and once with NOT EXISTS.

    
-- 4. Items that no player owns.
-- 5. Every player's COIN balance, showing 0 for players with no COIN row. Write the broken version first, then the fixed o

--GROUP BY/HAVING
-- 6. The number of players per country, largest first.
-- 7. Countries with more than 50 players.
-- 8. Each player's total inventory value per currency (quantity × price, grouped by player and currency).
-- 9. Items owned by at least 3 distinct players, with the owner count.

-- SUBQUERIES
-- 10. Players whose GEM balance is above the average GEM balance.
-- 11. Players who own at least one item priced in a premium currency. Use currencies.is_premium, not a hard-coded 'GEM'.

-- CTEs
-- 12. Players whose COIN-denominated inventory value is above their own country's average.

-- WINDOW FUNCTIONS
-- 13. The global leaderboard with ROW_NUMBER, RANK and DENSE_RANK side by side. Find your tied players in the output.
-- 14. Each player's rank within their country.
-- 15. "Players around me": given a player id, return that player plus the 5 above and 5 below by global rank, and include a column showing