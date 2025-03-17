package main

import (
	"advent-of-code/go"
	"fmt"
	"regexp"
	"strconv"
)

type Case struct {
	before      []int
	instruction []int
	after       []int
}

type ProgramLine struct {
	line []int
}

func toInts(strings []string) []int {
	var intNumbers []int
	for _, number := range strings {
		intNumber, _ := strconv.Atoi(number)
		intNumbers = append(intNumbers, intNumber)
	}
	return intNumbers
}

func parseInput(input []byte) ([]Case, []ProgramLine) {
	line := ""
	var useCase Case = Case{nil, nil, nil}
	var useCases []Case
	var program []ProgramLine
	newLines := 0
	readingProgram := false
	for _, v := range input {
		if !readingProgram {
			if v == 10 { // new line character
				newLines++
				if newLines >= 3 {
					readingProgram = true
				}
				numbers, _ := regexp.Compile("([0-9])+")
				before, _ := regexp.MatchString("Before:", line)
				after, _ := regexp.MatchString("After:", line)
				if before {
					numbersStrings := numbers.FindAllString(line, -1)
					useCase.before = toInts(numbersStrings)
				}
				if after {
					numbersStrings := numbers.FindAllString(line, -1)
					useCase.after = toInts(numbersStrings)
					useCases = append(useCases, useCase)
					useCase = Case{nil, nil, nil}
				}
				if !before && !after && line != "" {
					numbersStrings := numbers.FindAllString(line, -1)
					useCase.instruction = toInts(numbersStrings)
				}
				line = ""
			} else {
				line = line + string(v)
				newLines = 0
			}
		} else {
			if v == 10 {
				if line != "" {
					numbers, _ := regexp.Compile("([0-9])+")
					numbersStrings := numbers.FindAllString(line, -1)
					program = append(program, ProgramLine{toInts(numbersStrings)})
					line = ""
				}
			} else {
				line = line + string(v)
			}
		}
	}
	return useCases, program
}

func equal(reg []int, reg2 []int) bool {
	for i := range 4 {
		if reg[i] != reg2[i] {
			return false
		}
	}
	return true
}

func addr(reg []int, op []int, out []int) {
	out[op[3]] = reg[op[1]] + reg[op[2]]
}

func addi(reg []int, op []int, out []int) {
	out[op[3]] = reg[op[1]] + op[2]
}

func mulr(reg []int, op []int, out []int) {
	out[op[3]] = reg[op[1]] * reg[op[2]]
}

func muli(reg []int, op []int, out []int) {
	out[op[3]] = reg[op[1]] * op[2]
}

func banr(reg []int, op []int, out []int) {
	out[op[3]] = reg[op[1]] & reg[op[2]]
}

func bani(reg []int, op []int, out []int) {
	out[op[3]] = reg[op[1]] & op[2]
}
func borr(reg []int, op []int, out []int) {
	out[op[3]] = reg[op[1]] | reg[op[2]]
}

func bori(reg []int, op []int, out []int) {
	out[op[3]] = reg[op[1]] | op[2]
}

func setr(reg []int, op []int, out []int) {
	out[op[3]] = reg[op[1]]
}

func seti(reg []int, op []int, out []int) {
	out[op[3]] = op[1]
}

func gtir(reg []int, op []int, out []int) {
	if op[1] > reg[op[2]] {
		out[op[3]] = 1
	} else {
		out[op[3]] = 0
	}
}

func gtri(reg []int, op []int, out []int) {
	if reg[op[1]] > op[2] {
		out[op[3]] = 1
	} else {
		out[op[3]] = 0
	}
}

func gtrr(reg []int, op []int, out []int) {
	if reg[op[1]] > reg[op[2]] {
		out[op[3]] = 1
	} else {
		out[op[3]] = 0
	}
}
func eqir(reg []int, op []int, out []int) {
	if op[1] == reg[op[2]] {
		out[op[3]] = 1
	} else {
		out[op[3]] = 0
	}
}

func eqri(reg []int, op []int, out []int) {
	if reg[op[1]] == op[2] {
		out[op[3]] = 1
	} else {
		out[op[3]] = 0
	}
}

func eqrr(reg []int, op []int, out []int) {
	if reg[op[1]] == reg[op[2]] {
		out[op[3]] = 1
	} else {
		out[op[3]] = 0
	}
}

func exec(reg []int, op []int, expected []int, operations map[string]func([]int, []int, []int)) string {
	counter := 0
	result := []string{}
	for key, operation := range operations {
		out := []int{reg[0], reg[1], reg[2], reg[3]}
		operation(reg, op, out)
		if equal(out, expected) {
			result = append(result, key)
			counter++
		}
	}
	if len(result) == 1 {
		return result[0]
	}
	return ""
}

func main() {
	content := lib.ReadFile("input.txt")
	useCases, program := parseInput(content)
	operationToCode := make(map[int]string)
	codeToFunc := make(map[int]func([]int, []int, []int))
	operationsToCheck := make(map[string]func([]int, []int, []int))
	operationsToCheck["addr"] = addr
	operationsToCheck["addi"] = addi
	operationsToCheck["muli"] = muli
	operationsToCheck["mulr"] = mulr
	operationsToCheck["banr"] = banr
	operationsToCheck["bani"] = bani
	operationsToCheck["borr"] = borr
	operationsToCheck["bori"] = bori
	operationsToCheck["setr"] = setr
	operationsToCheck["seti"] = seti
	operationsToCheck["gtir"] = gtir
	operationsToCheck["gtri"] = gtri
	operationsToCheck["gtrr"] = gtrr
	operationsToCheck["eqrr"] = eqrr
	operationsToCheck["eqir"] = eqir
	operationsToCheck["eqri"] = eqri
	for _, useCase := range useCases {
		operation_name := exec(useCase.before, useCase.instruction, useCase.after, operationsToCheck)
		if operation_name != "" {
			operationToCode[useCase.instruction[0]] = operation_name
			codeToFunc[useCase.instruction[0]] = operationsToCheck[operation_name]
			delete(operationsToCheck, operation_name)
		}
	}

	registers := []int{0, 0, 0, 0}
	for _, line := range program {
		operation := codeToFunc[line.line[0]]
		operation(registers, line.line, registers)
	}
	fmt.Println(registers[0])
}
