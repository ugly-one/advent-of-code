import hashlib

id = "abbhdwsy"
index = 0
password = [0,0,0,0,0,0,0,0]
counter = 0
while True:
    to_hash = id + str(index)
    hash = hashlib.md5(to_hash.encode())
    result= hash.hexdigest()
    if result[:5] == "00000":
        char = result[5:6]
        if char.isdigit():
            position = int(result[5:6])
            if position > 7: 
                pass
            elif password[position] == 0:
                letter = result[6:7]
                password[position] = letter
                counter = counter + 1
                print(password)
    if counter == 8:
        break

    index = index + 1

for letter in password:
    print(letter, end="")

print()
