import hashlib

id = "abc"
id = "qzyelonm"
counter = 0

def is_one_char(string):
    same_chars = True
    previous = None
    for x in range(len(string)):
        if previous and previous != string[x]:
            same_chars = False
            break
        previous = string[x]
    return same_chars

def get_repetitive_characters(input, window_size, only_first = False):
    result = set()
    for i in range(len(input) - (window_size - 1)):
        window = input[i:i+window_size]
        if is_one_char(window):
            if only_first:
                return window[0]
            result.add(window[0])
    if only_first:
        return None
    return result

index_to_letter = {}
letter_to_indexes = {}
index = 0
correct_indexes = []
run = True
target_index = 64

log = dict()
while run:
    to_hash = id + str(index)
    # the loop for hashing is slow-ish - can we do better?
    for i in range(2017):
        hash = hashlib.md5(to_hash.encode())
        to_hash = hash.hexdigest()
    result = to_hash.lower()
    index_to_remove = index - 1001
    if index_to_remove in index_to_letter:
        letter_to_remove = index_to_letter[index_to_remove]
        letter_to_indexes[letter_to_remove].remove(index_to_remove)   
        index_to_letter.pop(index_to_remove)


    fives = get_repetitive_characters(result, 5)
    for five in fives:
        letter = five[0]
        if letter in letter_to_indexes:
            tracked_indexes = letter_to_indexes[letter]
            del letter_to_indexes[letter]
            for tracked_index in tracked_indexes:
                #print('current index: ' + str(index) + ' tracked_index ' + str(tracked_index) + ' diff: ' + str(index - (tracked_index)) + ' letter:  ' + letter + " current hash:" + result + " original hash: " + log[tracked_index])
                correct_indexes.append(tracked_index)
                counter = counter + 1
                index_to_letter.pop(tracked_index)

    tripple = get_repetitive_characters(result, 3, only_first = True)
    if tripple:
        index_to_letter[index] = tripple 
        if tripple in letter_to_indexes:
            letter_to_indexes[tripple].add(index)
        else:
            letter_to_indexes[tripple] = { index }

    if counter >= target_index + 30:
        sorted_valid_indexes = sorted(correct_indexes)
        print(sorted_valid_indexes[target_index-1])
        run = False
        break 
    
    index = index + 1


