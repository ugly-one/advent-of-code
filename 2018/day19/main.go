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

func parseInput(input []byte) []ProgramLine {
	line := ""
	var program []ProgramLine
	for _, v := range input {
		if v == 10 {
			numbers, _ := regexp.Compile("([0-9])+")
			numbersStrings := numbers.FindAllString(line, -1)
			program = append(program, ProgramLine{toInts(numbersStrings)})
			line = ""
		} else {
			line = line + string(v)
		}
	}
	return program
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

func exec(reg []int, operation string, arguments []int) {
	out := []int{reg[0], reg[1], reg[2], reg[3], reg[4], reg[5]}
	switch op := operation; op {
	case "seti":
		seti(reg, arguments, out)
	}
	return
}

func main() {
	content := lib.ReadFile("input.txt")
	program := parseInput(content)
	registers := []int{0, 0, 0, 0, 0, 0}
	for _, line := range program {
		exec(registers, line.operation, line.arguments)
	}
	fmt.Println(registers[0])
}
