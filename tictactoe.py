def main():
    print('=== Play Tic Tac Toe ===')
    g = Game()
    while True:
        g.display_board()
        while True:
            print(f"Play for {g.next_turn} (input format 'INT,INT')", end='')
            play_position = input(' > ')
            try:
                g.play_turn(play_position)
                break
            except ValueError as e:
                print(e)

        if g.winner:
            g.display_board()
            print(f"{g.winner} won the game!\nPlay again? ", end='')
            ans = input()
            if ans and ans.lower()[0] == 'y':
                g.reset_game()
            else:
                print('Goodbye!')
                break


class Game:
    def __init__(self):
        self._board = [
            [' ', ' ', ' '],
            [' ', ' ', ' '],
            [' ', ' ', ' '],
        ]
        self._next_turn = 'X'
        self._has_winner = False

    @property
    def next_turn(self):
        return self._next_turn

    @property
    def winner(self):
        if self._has_winner:
            return self.next_turn
        return ''

    def play_turn(self, position):
        if self._has_winner:
            raise ValueError('Game already has a winner.')

        row, col = self._parse_position(position)
        if self._board[row][col] != ' ':
            raise ValueError(f"'{position}' is already taken.")
        self._board[row][col] = self._next_turn

        if self._check_for_winner():
            self._has_winner = True
        else:
            self._alternate_turns()

    @staticmethod
    def _parse_position(position):
        try:
            row, col = position.split(',')
            row_position, col_position = int(row) - 1, int(col) - 1
        except (TypeError, ValueError):
            raise ValueError((
                f"'{position}' is of invalid format. "
                "Expected input in the format 'INT,INT'"
            ))

        if row_position not in range(3) or col_position not in range(3):
            raise ValueError(f"'{position}' is out of legal bounds.")

        return row_position, col_position

    def _alternate_turns(self):
        if self._next_turn == 'X':
            self._next_turn = 'O'
        else:
            self._next_turn = 'X'

    def _check_for_winner(self):
        for i in range(3):
            # check in row
            if self._board[i][0] == self._board[i][1] == self._board[i][2] != ' ':
                return True
            # check in column
            if self._board[0][i] == self._board[1][i] == self._board[2][i] != ' ':
                return True

        # check diagonal (starting top-left)
        if self._board[0][0] == self._board[1][1] == self._board[2][2] != ' ':
            return True
        # check diagonal (starting top-right)
        if self._board[0][2] == self._board[1][1] == self._board[2][0] != ' ':
            return True

        return False

    def reset_game(self):
        for row in self._board:
            for i in range(3):
                row[i] = ' '
        self._next_turn = 'X'
        self._has_winner = False

    def display_board(self):
        print('+---+---+---+')
        for row_ix, row in enumerate(self._board, start=1):
            print('|', end='')
            for col_ix, cell in enumerate(row, start=1):
                if cell == ' ':
                    print(f"{row_ix},{col_ix}|", end='')
                else:
                    print(f" {cell} |", end='')
            print('\n+---+---+---+')


if __name__ == '__main__':
    main()
