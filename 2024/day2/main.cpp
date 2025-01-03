#include <cstdint>
#include <cstdio>
#include <cstdlib>
#include <vector>

#include "parser.cpp"

uint8_t isSafe(std::vector<int> line, int8_t indexToSkip){
    uint8_t safe = 1;
    int sign = 0;
    int firstIndex = 0;
    int secondIndex = 0;
    for(int j = 0; j < line.size() - 1; j++){
        if (indexToSkip == 0 && j == 0)
        {
            continue;
        }
        if (indexToSkip == (line.size() - 1) && j == (line.size() - 2)){
            continue;
        }

        firstIndex = j;
        secondIndex = j+1;
        if (j+1 == indexToSkip)
        {
            secondIndex++;
            j++;
        }
       
        int diff = line[secondIndex] - line[firstIndex]; 
        uint absDiff = 0;
        if (diff < 0){
            absDiff = -diff;
        }
        else{
            absDiff = diff;
        }
        if (absDiff < 1 || absDiff > 3)
        {
            safe = 0;
            break;
        }
        if (sign == 0)
        {
            if (diff < 0){
                sign = -1;
            }
            else{
                sign = 1;
            }
        }
        else
        {
            int newSign = 0;
            if (diff < 0){
                newSign = -1;
            }
            else{
                newSign = 1;
            }
            if (newSign != sign){
                safe = 0;
                break;
            }
        }
    }
    return safe;
}

int main(int argc, char* argv[])
{
    if (argc != 2){
        printf("usage: day2 [input file]\n");
        return -1;
    }
    const char* fileName = argv[1];

    File file = loadFile(fileName);
    std::vector<std::vector<int>> *lines = parseFile(file.data, file.size);

    uint safeCounter = 0;
    for(int i = 0; i < lines->size(); i++){

        std::vector<int> line = lines->at(i);
        if (isSafe(line, -1)){
            safeCounter++;
        }
        else{
            uint safeAfterSkipCounter = 0;
            for(uint indexToSkip = 0; indexToSkip < line.size(); indexToSkip++){
                if (isSafe(line, indexToSkip)){
                    safeAfterSkipCounter++;
                }
            }
            if (safeAfterSkipCounter){
                safeCounter++;
            }
        }
    }

    printf("%d\n", safeCounter);
}
