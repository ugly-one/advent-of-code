#include "../parser.cpp"


bool within(int index, int boundary){
    if (index < 0) return false;
    if (index > boundary) return false;
    return true;
};

bool hasletter(int newIndex, int boundary, uint8_t *data, char letter){
    if (within(newIndex, boundary - 1)){
        if (data[newIndex] == letter){
            return true;
        }
    }
    return false;
}

bool hasMas(int index, int boundary, int x, int y, uint8_t *data, uint width){
    int newIndex = index + x + width * y; 
    if (hasletter(newIndex, boundary, data, 'M')){
        int newIndex2 = newIndex + x + width * y;
        if (hasletter(newIndex2, boundary, data, 'A')){
            int newIndex3 = newIndex2 + x + width * y;
            if (hasletter(newIndex3, boundary, data, 'S')){
                return true;
            }
        }
    }
    return false;
}

int main(int argc, char* argv[])
{
    File file = loadFile(argv[1]);

    uint width = 0;
    for (int i = 0; i < file.size; i++){
        char letter = file.data[i];
        if (letter == '\n'){
            width = i + 1;
            break;
        }
    }
    uint counter = 0;

    for (int i = 0; i < file.size; i++)
    {
        char letter = file.data[i];
        if (letter == 'X'){
            // pick direction and check if XMAS exists
            int x = 0;
            int y = -1;
            if (hasMas(i, file.size, x, y, file.data, width)){
                counter++;
            }
            x = 1;
            y = -1;
            if (hasMas(i, file.size, x, y, file.data, width)){
                counter++;
            }
            x = 1;
            y = 0;
            if (hasMas(i, file.size, x, y, file.data, width)){
                counter++;
            }
            x = 1;
            y = 1;
            if (hasMas(i, file.size, x, y, file.data, width)){
                counter++;
            }
            x = 0;
            y = 1;
            if (hasMas(i, file.size, x, y, file.data, width)){
                counter++;
            }
            x = -1;
            y = 1;
            if (hasMas(i, file.size, x, y, file.data, width)){
                counter++;
            }
            x = -1;
            y = 0;
            if (hasMas(i, file.size, x, y, file.data, width)){
                counter++;
            }
            x = -1;
            y = -1;
            if (hasMas(i, file.size, x, y, file.data, width)){
                counter++;
            }
        }
    }

    printf("%d", counter);
    return 0;
}
