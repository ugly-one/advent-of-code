#include "../parser.cpp"

bool within(int index, int boundary){
    if (index < 0) return false;
    if (index > boundary) return false;
    return true;
};

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
        uint size = file.size;
        uint8_t *data = file.data;
        if (letter == 'A'){
            int index1 = i + 1 + width * -1;
            int index2 = i + 1 + width * 1;
            int index3 = i + -1 + width * 1;
            int index4 = i + -1 + width * -1;

            if (!within(index1, size) || !within(index2, size) || !within(index3, size) || !within(index4, size)){
                continue;
            }

            if (data[index1] == 'M' && data[index3] == 'S' && data[index2] == 'M' && data[index4] == 'S'){
                counter ++;
            }
            if (data[index1] == 'S' && data[index3] == 'M' && data[index2] == 'M' && data[index4] == 'S'){
                counter ++;
            }
            if (data[index1] == 'M' && data[index3] == 'S' && data[index2] == 'S' && data[index4] == 'M'){
                counter ++;
            }
            if (data[index1] == 'S' && data[index3] == 'M' && data[index2] == 'S' && data[index4] == 'M'){
                counter ++;
            }
        }
    }

    printf("%d", counter);
    return 0;
}
