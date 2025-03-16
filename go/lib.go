package lib

import (
	"fmt"
	"os"
	"path"
	"runtime"
)

func ReadFile(filename string) []byte {
	content, _ := os.ReadFile(getInputDir() + filename)
	return content
}

func getInputDir() string {
	_, filename, _, ok := runtime.Caller(2)
	if !ok {
		panic("No caller information")
	}
	fmt.Printf("Filename : %q, Dir : %q\n", filename, path.Dir(filename))

	var a string = ""
	dir := filename
	for true {
		dir = path.Dir(dir)
		base := path.Base(dir)
		if base == "advent-of-code" {
			break
		}
		a = path.Base(dir) + "/" + a
	}

	inputDir := path.Dir(dir) + "/advent-of-code-input/" + a + "/"
	return inputDir
}
