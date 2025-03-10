local lib = require('lib')
local input = lib.getLines()

local player = { hit = 50, mana = 500, }
local boss = { hit = 51, damage = 9 }

local spells = {}
spells['magic missle'] = { cost = 53, instant = true, damage = 4, }
spells['drain'] = { cost = 73, instant = true, damage = 2, heal = 2 }
spells['shield'] = { cost = 113, turns = 6, armor_once = 7 }
spells['poison'] = { cost = 173, damage = 3, turns = 6 }
spells['recharge'] = { cost = 229, mana = 101, turns = 5 }

local available_spells = lib.copy_dic(spells)
local ongoing_effects = {}

local function do_effects(ongoing_effects, player, boss)
  for name, turns in pairs(ongoing_effects) do
    if turns > 0 then
      local effect = spells[name]
      if effect.armor_once then player.armor = effect.armor_once end
      if effect.damage then boss.hit = boss.hit - effect.damage end
      if effect.mana then player.mana = player.mana + effect.mana end
      --
      ongoing_effects[name] = turns - 1
    end
  end
end

local function clean_up(ongoing_effects, available_spells)
  -- clean up used effects
  for name, turns in pairs(ongoing_effects) do
    if turns == 0 then
      ongoing_effects[name] = nil
      available_spells[name] = spells[name]
    end
  end
end

local min = nil
local function is_boss_alive(boss, mana_spent)
  if boss.hit <= 0 then
    if min == nil then
      min = mana_spent
    else
      if mana_spent < min then
        min = mana_spent
      end
    end
    return false
  end
  return true
end

local function make_turn(player, boss, available_spells, ongoing_effects, mana_spent, cast_spells)
  if min ~= nil and mana_spent >= min then return end

  player.hit = player.hit - 1
  if player.hit < 1 then return end

  do_effects(ongoing_effects, player, boss)
  clean_up(ongoing_effects, available_spells)

  if is_boss_alive(boss, mana_spent) then
    for name, spell in pairs(available_spells) do
      local player = { hit = player.hit, mana = player.mana, armor = 0 }
      local boss = { hit = boss.hit, damage = boss.damage }
      local ongoing_effects = lib.copy_dic(ongoing_effects)
      local available_spells = lib.copy_dic(available_spells)
      if player.mana >= spell.cost then
        player.mana = player.mana - spell.cost
        local mana_spent = mana_spent + spell.cost
        if spell.instant then
          boss.hit = boss.hit - spell.damage
          if spell.heal then
            player.hit = player.hit + spell.heal
          end
        else
          ongoing_effects[name] = spell.turns
          available_spells[name] = nil
        end

        do_effects(ongoing_effects, player, boss)
        if is_boss_alive(boss, mana_spent) then
          clean_up(ongoing_effects, available_spells)

          local damage = boss.damage - player.armor
          if damage < 1 then damage = 1 end
          player.hit = player.hit - damage
          if player.hit > 0 then
            make_turn(player, boss, available_spells, ongoing_effects, mana_spent, cast_spells .. ' ' .. name)
          end
        end
      end
    end
  end
end

make_turn(player, boss, available_spells, ongoing_effects, 0, "")
print(min)
