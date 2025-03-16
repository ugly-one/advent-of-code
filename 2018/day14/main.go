package main

import (
	"fmt"
	"strconv"
	"strings"
)

type Node struct {
	data int
	next *Node
}

func main() {
	part1()
	part2()
}

func part1() {

	root := &Node{3, nil}
	next := &Node{7, root}
	root.next = next
	last := next

	elf1 := root
	elf2 := next
	target := 765071
	count := 2
	var answer strings.Builder
	for count < target+10 {
		new_recipe := elf1.data + elf2.data
		if new_recipe < 10 {
			last = addAfter(last, new_recipe)
			count++
			if count > target {
				answer.WriteString(strconv.Itoa(last.data))
			}
		} else {
			last = addAfter(last, 1)
			count++
			if count > target {
				answer.WriteString(strconv.Itoa(last.data))
			}
			last = addAfter(last, new_recipe-10)
			count++
			if count > target && count < target+10 {
				answer.WriteString(strconv.Itoa(last.data))
			}
		}

		elf1 = getNext(elf1, elf1.data+1)
		elf2 = getNext(elf2, elf2.data+1)
	}

	fmt.Println(answer.String())

}

func part2() {
	root := &Node{3, nil}
	next := &Node{7, root}
	root.next = next
	last := next

	elf1 := root
	elf2 := next
	count := 2
	target := [6]int{7, 6, 5, 0, 7, 1}
	target_len := len(target)
	digit_to_check := 0
	for true {
		new_recipe := elf1.data + elf2.data
		if new_recipe < 10 {
			last = addAfter(last, new_recipe)
			count++

			if target[digit_to_check] == last.data {
				digit_to_check++
			} else {
				digit_to_check = 0
			}
			if digit_to_check == target_len {
				break
			}
		} else {
			last = addAfter(last, 1)
			count++
			if target[digit_to_check] == last.data {
				digit_to_check++
			} else {
				digit_to_check = 0
			}
			if digit_to_check == target_len {
				break
			}
			last = addAfter(last, new_recipe-10)
			count++
			if target[digit_to_check] == last.data {
				digit_to_check++
			} else {
				digit_to_check = 0
			}
			if digit_to_check == target_len {
				break
			}

		}

		elf1 = getNext(elf1, elf1.data+1)
		elf2 = getNext(elf2, elf2.data+1)
	}

	fmt.Println(count - target_len)
}

func getNext(node *Node, steps int) *Node {
	i := 1
	for i <= steps {
		node = node.next
		i++
	}
	return node
}

func addAfter(node *Node, data int) *Node {
	new := &Node{data, nil}
	new.next = node.next
	node.next = new
	return new
}

func print(node *Node) {
	start := node
	for {
		fmt.Print(node.data, ",")
		node = node.next
		if node == start {
			break
		}
	}
	fmt.Println()
}
