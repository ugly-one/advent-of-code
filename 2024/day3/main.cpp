#include <ctype.h>
#include "../parser.cpp"

enum state
{
    unknown,
    m,
    u,
    l,
    open,
    number1,
    comma,
    number2,
    close
};

uint8_t length(const char* number)
{
    uint length = 0;
    while(*(number++) != '\0')
        length++;
    return length;
}

int main(int argc, char* argv[])
{
    File file = loadFile(argv[1]);
    state state = unknown;
    char n1[4] = {'\0'};
    char n2[4] = {'\0'};
    uint8_t index = 0;

    uint sum = 0;

    for (int i = 0; i < file.size; i++)
    {
        char c = file.data[i];
        if (c == 'm' && state == unknown){
            state = m; 
        }
        else if (c == 'u' && state == m){
            state = u;
        }
        else if (c == 'l' && state == u){
            state = l;
        }
        else if (c == '(' && state == l){
            state = open;
        }
        else if (isdigit(c) && state == open){
            state = number1;
            clear(n1,4);
            index = 0;
            n1[index] = c;
            index++;
        }
        else if (isdigit(c) && state == number1 && length(n1) < 3){
            n1[index] = c;
            index++;
        }
        else if (c == ',' && state == number1 && length(n1) < 4){
            index = 0;
            state = comma;
        }
        else if (isdigit(c) && state == comma){
            state = number2;
            clear(n2,4);
            index = 0;
            n2[index] = c;
            index++;
        }
        else if (isdigit(c) && state == number2 && length(n2) < 3){
            n2[index] = c;
            index++;
        }
        else if (c == ')' && state == number2 && length(n2) < 4)
        {
            n2[index] = c;
            sum = sum + atoi(n1) * atoi(n2);
            state = unknown;
            
        }
        else{
            state = unknown;
        }
    }

    printf("%d", sum);
    return 0;
}
