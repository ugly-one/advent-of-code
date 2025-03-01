local lib = require('lib')
local input = lib.getLines()

local weapons = {
  { cost = 8,  damage = 4, armor = 0 },
  { cost = 10, damage = 5, armor = 0 },
  { cost = 25, damage = 6, armor = 0 },
  { cost = 40, damage = 7, armor = 0 },
  { cost = 74, damage = 8, armor = 0 },
}

local armor = {
  { cost = 0,   damage = 0, armor = 0 },
  { cost = 13,  damage = 0, armor = 1 },
  { cost = 31,  damage = 0, armor = 2 },
  { cost = 53,  damage = 0, armor = 3 },
  { cost = 75,  damage = 0, armor = 4 },
  { cost = 102, damage = 0, armor = 5 },
}

local rings = {
  { cost = 0,   damage = 0, armor = 0 },
  { cost = 0,   damage = 0, armor = 0 },
  { cost = 25,  damage = 1, armor = 0 },
  { cost = 50,  damage = 2, armor = 0 },
  { cost = 100, damage = 3, armor = 0 },
  { cost = 20,  damage = 0, armor = 1 },
  { cost = 40,  damage = 0, armor = 2 },
  { cost = 80,  damage = 0, armor = 3 },
}

local function run_game(attacker, victim)
  while true do
    local damage = attacker.damage - victim.armor
    if damage <= 0 then damage = 1 end
    victim.hit = victim.hit - damage
    if victim.hit <= 0 then break end
    local temp = attacker
    attacker = victim
    victim = temp
  end
  if victim.boss then return true else return false end
end

local max_cost = nil
for _, weapon in ipairs(weapons) do
  for _, armor in ipairs(armor) do
    for i, ring1 in ipairs(rings) do
      local second_hand_rings = lib.copy_table(rings)
      table.remove(second_hand_rings, i)
      for _, ring2 in ipairs(second_hand_rings) do
        local player = {
          hit = 100,
          damage = weapon.damage + ring1.damage + ring2.damage,
          armor = armor.armor + ring2.armor + ring1.armor
        }
        local cost = weapon.cost + armor.cost + ring1.cost + ring2.cost
        local boss = { boss = true, hit = 103, damage = 9, armor = 2 }
        local result = run_game(player, boss)
        if result == false then
          if max_cost == nil then
            max_cost = cost
          else
            if cost > max_cost then
              max_cost = cost
            end
          end
        end
      end
    end
  end
end
print(max_cost)
