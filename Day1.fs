module Day1

open Day1_input

type Bla = {
        Current : int
        High : int
        Medium : int
        Low : int

    }

let check state item = 
    if item <> 0 then {
        state with Current = (state.Current + item)} 
    else
        if state.Current > state.High 
            then {High = state.Current; Medium = state.High; Low = state.Medium; Current = 0} 
        else if state.Current > state.Medium
            then {High = state.High; Medium = state.Current; Low = state.Medium; Current = 0}
        else if state.Current > state.Low
            then {High = state.High; Medium = state.Medium; Low = state.Current; Current = 0}
        else {state with Current = 0} 

let total = 
    Array.fold (fun state item -> check state item) {Current = 0; High = 0; Medium = 0; Low = 0} realInput


let run = (total.High + total.Medium + total.Low)