#!/bin/bash

func (){
grep -n "Program.fs" 2021-fsharp.fsproj
}
lineMatched=$(func)
# if line > 99 we should take 3 characters
line=${lineMatched:0:2}
day="Day"$1
mkdir $day
touch $day/input.txt
touch $day/$day.fs
echo "module "$day > $day/$day.fs
echo "" >> $day/$day.fs
echo "let run () =" >> $day/$day.fs
echo "   ()" >> $day/$day.fs

sed -i $line' i \ \ \ \ <Content Include="'$day'\\input.txt"><CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory></Content>' 2021-fsharp.fsproj
sed -i $line' i \ \ \ \ <Compile Include="'$day'\\'$day'.fs"/>' 2021-fsharp.fsproj

echo $day".run()" > Program.fs
