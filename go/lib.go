package lib

import (
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
