#!/bin/bash

door_id='abc'
index=3231929
pass=""
for (( ; ; ))
do
	to_hash=$door_id$index
	result=$(echo -n $to_hash | md5sum)
	sub="${result:0:5}"
	if [ "$sub" == "00000" ]; then
		pass=$pass${result:5:1}
		echo $pass
	fi
	if [ ${#pass} -eq 2 ]; then
		break
	fi
	index=$((index + 1))
done
