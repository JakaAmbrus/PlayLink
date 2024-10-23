//start screen
const overlay = document.getElementById("overlay");
const startBtn = document.querySelector(".start-btn");
const startImg = document.getElementById("start-img");
//choose game mode 
const chooseDifficultyScreen = document.getElementById("choose-difficulty-screen");
const easyBtn = document.getElementById("easy-btn");
const midBtn = document.getElementById("mid-btn");
const impossibleBtn = document.getElementById("impossible-btn");
//choose turn screen
const chooseTurnScreen = document.getElementById("choose-turn-screen");
const xSelect = document.getElementById("x-select");
const oSelect = document.getElementById("o-select");
const xToggleSelect = document.getElementById("x-turn-button");
const oToggleSelect = document.getElementById("o-turn-button");
const bossDiffText = document.getElementById("boss-diff-text");
//game screen
const gameScreen = document.getElementById("game-screen");
const pvpBtn = document.getElementById("pvp");
const squares = document.querySelectorAll(".square");
const square0 = document.getElementById("square-0");
const square1 = document.getElementById("square-1");
const square2 = document.getElementById("square-2");
const square3 = document.getElementById("square-3");
const square4 = document.getElementById("square-4");
const square5 = document.getElementById("square-5");
const square6 = document.getElementById("square-6");
const square7 = document.getElementById("square-7");
const square8 = document.getElementById("square-8");
//end screens
const xWinScreen = document.getElementById("x-wins-screen");
const oWinScreen = document.getElementById("o-wins-screen");
const winScreen = document.getElementById("win-screen");
const loseScreen = document.getElementById("lose-screen");
const drawScreen = document.getElementById("draw-screen");
const bossScreen1 = document.getElementById("boss-screen-1");
const bossScreen2 = document.getElementById("boss-screen-2");
const bossScreen3 = document.getElementById("boss-screen-3");
const happyEndScreen = document.getElementById("happy-end-screen");

//audio
let gameAudio = document.querySelectorAll('.game-audio');
let muteButton = document.querySelector('.mute-png');
const transitionAudio = document.getElementById("transition");
const dangerAudio = document.getElementById("danger");
const warmupAudio = document.getElementById("warm-up");
const standardAudio = document.getElementById("standard");
const pvpAudio = document.getElementById("pvp-audio");
const bonesAudio = document.getElementById("bones");
const winAudio = document.getElementById("win-audio");
const loseAudio = document.getElementById("lose-audio");
const drawAudio = document.getElementById("draw-audio");
const clickAudio = document.getElementById("click-audio");
const bossStartAudio = document.getElementById("boss-1-audio");
const bossTransitionAudio = document.getElementById("boss-transition-audio");
const bossBeatAudio = document.getElementById("boss-beat-audio");
//color
const rootStyles = getComputedStyle(document.documentElement);
const primaryColor = rootStyles.getPropertyValue('--primary-color');

let mode; //to switch between difficulties
let bossHealth; //to determine when you can beat the impossible boss
//screen transition
function transitionScreen(current, next)
{
  if(next == overlay || next == chooseDifficultyScreen || next == gameScreen || next ==chooseTurnScreen)
  {
  current.style.display = 'none';
  next.style.display = 'grid';
  }
  else{
  current.style.display = 'none';
  next.style.display = 'flex';
  }
  if (next == chooseDifficultyScreen) {
    clearBoard();
  }
}
//button transition assignments
startBtn.addEventListener('click', function() {
  transitionScreen(overlay, chooseDifficultyScreen);
  transitionAudio.play()
});
easyBtn.addEventListener('click', function() {
  transitionScreen(chooseDifficultyScreen, chooseTurnScreen);
  pvpAudio.play();
  muteButton.style.display = 'none';
  mode = 'easy';
  bossDiffText.textContent = ''
});
midBtn.addEventListener('click', function() {
  transitionScreen(chooseDifficultyScreen, chooseTurnScreen);
  pvpAudio.play();
  muteButton.style.display = 'none';
  mode = 'mid';
  bossDiffText.textContent = ''
});
impossibleBtn.addEventListener('click', function() {
  transitionScreen(chooseDifficultyScreen, chooseTurnScreen);
  bossScreen1.style.display = 'flex';
  chooseTurnScreen.style.opacity = "0";
  bossStartAudio.play();
  muteButton.style.display = 'none';
  mode = 'impossible';
  bossHealth = 2;
  bossDiffText.textContent = 'IMPOSSIBLE TO WIN'
  setTimeout(() => {
    bossScreen1.style.display = 'none';
    chooseTurnScreen.style.opacity = "1";
  }, 2750)
});
pvpBtn.addEventListener('click', function() {
  transitionScreen(chooseDifficultyScreen, chooseTurnScreen);
  pvpAudio.play();
  muteButton.style.display = 'none';
  mode = 'pvp';
  bossDiffText.textContent = ''
});
//turn selection
xSelect.addEventListener('click', () => {
  player = 'x';
  computer = 'o';
  turnPlayerUndefined()
  transitionScreen(chooseTurnScreen, gameScreen);
  transitionAudio.play();
   if(mode == 'easy' && turnPlayer == computer){
    gameScreen.style.pointerEvents = 'none'
    setTimeout(() => {
      gameScreen.style.pointerEvents = 'all'
      computerMove();
    }, 400);
  }
  else if(mode == 'mid' && turnPlayer == computer){
    gameScreen.style.pointerEvents = 'none'
    setTimeout(() => {
      gameScreen.style.pointerEvents = 'all'
      computerMoveMid(squares);
    }, 400);
  }
  else if(mode == 'impossible' && turnPlayer == computer){
    gameScreen.style.pointerEvents = 'none'
    setTimeout(() => {
      gameScreen.style.pointerEvents = 'all'
      computerMoveMax()
    }, 400);
  }
});
oSelect.addEventListener('click', () => {
  player = 'o';
  computer = 'x';
  turnPlayerUndefined()
  transitionScreen(chooseTurnScreen, gameScreen);
  transitionAudio.play();
  if(mode == 'easy' && turnPlayer == computer){
    gameScreen.style.pointerEvents = 'none'
    setTimeout(() => {
      gameScreen.style.pointerEvents = 'all'
      computerMove();
    }, 400);
  }
  else if(mode == 'mid' && turnPlayer == computer){
    gameScreen.style.pointerEvents = 'none'
    setTimeout(() => {
      gameScreen.style.pointerEvents = 'all'
      computerMoveMid(squares);
    }, 400);
  }
  else if(mode == 'impossible' && turnPlayer == computer){
    gameScreen.style.pointerEvents = 'none'
    setTimeout(() => {
      gameScreen.style.pointerEvents = 'all'
      computerMoveMax()
    }, 400);
  }
});

//starting player selection

let remainer;

xToggleSelect.addEventListener('click', function() {
  selectStarterToggle(xToggleSelect, oToggleSelect, 'x');
});

oToggleSelect.addEventListener('click', () => {
  selectStarterToggle(oToggleSelect, xToggleSelect, 'o');
});


function selectStarterToggle(selected, other, n){
  selected.classList.add("turn-select-toggle");
  other.classList.remove("turn-select-toggle");
  selected.style.pointerEvents = 'none';
  other.style.pointerEvents = 'all';
  selected.style.cursor = 'none'
  other.style.cursor = 'pointer'
  turnPlayer = n;
  remainer = turnPlayer;
  clickAudio.play()
  
}
function turnPlayerUndefined(){
  if(turnPlayer == undefined){
    turnPlayer = 'x';
  }
}

//difficulty hover sounds

impossibleBtn.addEventListener('mouseover', function(){
  dangerAudio.play();
});
easyBtn.addEventListener('mouseover', function(){
  warmupAudio.play();
});
midBtn.addEventListener('mouseover', function(){
  standardAudio.play();
});
startBtn.addEventListener('mouseover', function(){
  bonesAudio.play();
});
// audio buttons
let isMuted = false;

muteButton.addEventListener('click', function() {
  isMuted = !isMuted;

  gameAudio.forEach(function(audio) {
    audio.muted = isMuted; 
  });

  muteButton.setAttribute('src', isMuted ? 'img/muted.png' : 'img/unmuted.png'); 
});

//game logic//
let turnPlayer;
let player ;
let computer ;
let gameField = ['', '', '', '', '', '', '', '', ''];
squares.forEach((square, i) => {
  square.addEventListener('mouseenter', () => {
    if (gameField[i] === '') {
      square.style.color = 'transparent';
      square.style.webkitTextStroke = '1px white';
      square.style.textStroke = '1px white';
      square.textContent = turnPlayer;
      square.style.backgroundColor = 'transparent';
    }
  });

  square.addEventListener('mouseleave', () => {
    if (gameField[i] === '') {
      square.style.color = primaryColor;
      square.style.webkitTextStroke = '';
      square.style.textStroke = '';
      square.style.backgroundColor = '';
      square.textContent = '';
    }
  });

  square.addEventListener('click', () => {
    if (gameField[i] === '') {
      square.style.color = primaryColor;
      square.style.webkitTextStroke = '';
      square.style.textStroke = '';
      square.style.backgroundColor = '';
      if (mode == 'pvp') {
        gameField[i] = turnPlayer;
        square.textContent = turnPlayer;
        clickAudio.play();
        turnPlayer = turnPlayer === 'x' ? 'o' : 'x';
        checkOutcome();
      } else {
        makeMove(i);
      }
    }
  });
});

let gameEnded;
function checkOutcome() { //checks the winning outcome
  const winningCombinations = [
    // horizontally
    [0, 1, 2], 
    [3, 4, 5], 
    [6, 7, 8],
    // vertically
    [0, 3, 6], 
    [1, 4, 7], 
    [2, 5, 8], 
    // diagonally
    [0, 4, 8], 
    [2, 4, 6]              
  ];
  for (const combination of winningCombinations) {
    const [a, b, c] = combination;
    if (gameField[a] !== '' && gameField[a] === gameField[b] && gameField[b] === gameField[c]) {
      if (mode !== 'pvp') {
        if(bossHealth == 0 && gameField[a] === player){
          gameEnded = true;
          let winningSquares = [a, b, c];
          happyEndingTransition();
          setTimeout(() => {
            highlightWinningSquares(winningSquares)
          },80);
        }
        else{
          gameEnded = true;
          let winningSquares = [a, b, c];
            gameField[a] === player ? endScreenTransitions(winScreen, winAudio) : endScreenTransitions(loseScreen, loseAudio);
            setTimeout(() => {
              highlightWinningSquares(winningSquares)
            },80);
        }


      } else{
        let winningSquares = [a, b, c];
          gameField[a] === 'x' ? endScreenTransitions(xWinScreen, winAudio) : endScreenTransitions(oWinScreen, winAudio);
          setTimeout(() => {
            highlightWinningSquares(winningSquares)
          },80);
          
      }
     
      return true;
    }
  }

  const fullBoard = gameField.every(square => square !== '');
  if (fullBoard) {
    if(mode == 'impossible'){
      gameEnded = true;
      if(bossHealth == 2){
        bossHealth = 1;
        bossBattleTransitions(bossScreen2);
        setTimeout(() => {
          highlightAllSquares();
        },80);
      }
      else if(bossHealth == 1){
        bossHealth = 0;
        bossBattleTransitions(bossScreen3);
        setTimeout(() => {
          highlightAllSquares();
        },80);
      }
    }
    else{
      endScreenTransitions(drawScreen, drawAudio);
      setTimeout(() => {
        highlightAllSquares();
      },80);
      gameEnded = true;
    }
    return 'tie';
  }

  return false;
}

function makeMove(i) { //the part that handles movement for non pvp modes
  if (gameEnded) return;
  gameField[i] = turnPlayer;
  squares[i].textContent = turnPlayer;
  clickAudio.play()
  squares[i].removeEventListener('click', makeMove); 
  turnPlayer = turnPlayer === player ? computer : player;
  checkOutcome()
  if (gameEnded) return;
  if (turnPlayer === computer) {
    gameScreen.style.pointerEvents = 'none';
    setTimeout(() => {
      if (mode === 'easy') {
        if (gameEnded) return;
        computerMove();
      } else if (mode === 'mid') {
        if (gameEnded) return;
        computerMoveMid(squares);
      }
      else if(mode == 'impossible'){
        if (gameEnded) return;
        computerMoveMax();
      }
      gameScreen.style.pointerEvents = 'all';
    }, 600);
  }
}

function computerMove() { //easy difficulty logic
  const emptyFields = gameField.reduce((acc, value, index) => {
    if (value === '') {
      acc.push(index);
    }
    return acc;
  }, []);
  let selectedIndex;
  if (gameEnded) return;
  selectedIndex = Math.floor(Math.random() * emptyFields.length);
  const selectedField = emptyFields[selectedIndex];
  gameField[selectedField] = turnPlayer;
  squares[selectedField].textContent = turnPlayer;
  turnPlayer = player;
  clickAudio.play()
  checkOutcome();

}

function computerMoveMid(squares) { //mid difficulty logic
  const emptyFields = gameField.reduce((acc, value, index) => {
    if (value === "") {
      acc.push(index);
    }
    return acc;
  }, []);

  let selectedIndex;

  for (const field of emptyFields) {  // Check for a winning move
    gameField[field] = turnPlayer;
    if (checkOutcome()) {
      selectedIndex = field;
      break;
    }
    gameField[field] = "";
  }

  if (selectedIndex === undefined) {
    if (emptyFields.length > 1) {
      selectedIndex = Math.floor(Math.random() * emptyFields.length);
      selectedIndex = emptyFields[selectedIndex];
   
  } else {
      selectedIndex = emptyFields[0];
  }
}
    gameField[selectedIndex] = turnPlayer;
    squares[selectedIndex].textContent = turnPlayer;
    turnPlayer = player;
    clickAudio.play()
    checkOutcome()
  }

//max difficulty logic


function computerMoveMax() {
  const emptyFields = gameField.reduce((array, value, i) => {
    if (value === "") {
      array.push(i);
    }
    return array;
  }, []);

  let selectedIndex = -Infinity;
  let bestMovesArr = [];

  for (const field of emptyFields) {
    gameField[field] = computer;

    const score = minmaxAlgorithm(gameField, 0, false, -Infinity, Infinity);

    gameField[field] = "";

    if (score > selectedIndex) {
      selectedIndex = score;
      bestMovesArr = [field];
    } else if (score === selectedIndex) {
      bestMovesArr.push(field);
    }
  }

  const randomIndex = Math.floor(Math.random() * bestMovesArr.length);
  const bestMove = bestMovesArr[randomIndex];

  gameField[bestMove] = computer;
  squares[bestMove].textContent = computer;
  turnPlayer = player;
  clickAudio.play();
  checkOutcome();
}

// minmax
function minmaxAlgorithm(playField, depth, maximizingPlayer, alpha, beta) {
  if (checkOutcomeMax(playField, player)) {
    return -10 + depth; //add value when player wins
  }

  if (checkOutcomeMax(playField, computer)) {
    return 10 - depth; //add value when player wins
  }

  if (isFieldFull(playField)) {
    return 0; // this is a draw, no change
  }

  let bestScore = maximizingPlayer ? -Infinity : Infinity;

  for (let i = 0; i < playField.length; i++) {
    if (playField[i] === '') {
      playField[i] = maximizingPlayer ? computer : player;
      let score = minmaxAlgorithm(playField, depth + 1, !maximizingPlayer, alpha, beta);
      playField[i] = '';

      if (maximizingPlayer) {
        bestScore = Math.max(score, bestScore);
        alpha = Math.max(alpha, bestScore);
      } else {
        bestScore = Math.min(score, bestScore);
        beta = Math.min(beta, bestScore);
      }

      if (beta <= alpha) {
        break; // if beta is not bigger there is no point in continuing
      }
    }
  }

  return bestScore;
}

function isFieldFull(playField) {
  return playField.every(field => field !== '');
}

function checkOutcomeMax(playField, selectedPlayer) { //doing this again with selectedPlayer in mind
  const winningCombinations = [
    // horizontally
    [0, 1, 2], 
    [3, 4, 5], 
    [6, 7, 8],
    // vertically
    [0, 3, 6], 
    [1, 4, 7], 
    [2, 5, 8], 
    // diagonally
    [0, 4, 8], 
    [2, 4, 6]  
  ];

  for (const combination of winningCombinations) {
    const [a, b, c] = combination;
    if (playField[a] === selectedPlayer && playField[b] === selectedPlayer && playField[c] === selectedPlayer) {
      return true; // if the selected player has a winning combination
    }
  }

  return false; // if the selected player has no winning combination
}
//clears the board
function clearBoard() {
  squares.forEach((square, i) => {
    square.textContent = '';
    gameField[i] = '';
    turnPlayer = remainer;
  });
  mode = undefined;
}


//transitions of the outcome
function endScreenTransitions(outcome, audio){
  gameScreen.style.pointerEvents = 'none';
  setTimeout(() => {
    transitionScreen(gameScreen, outcome);
    setTimeout(() => {outcome.style.opacity = "1";}, 100)
    gameScreen.style.pointerEvents = 'all';
    audio.play();
    clearBoard();
    setTimeout(() => {
      outcome.style.opacity = "0";
      transitionScreen(outcome, chooseDifficultyScreen);
      muteButton.style.display = 'inline-block';
      gameEnded = false;
      bossHealth = 2;
    }, 3000);
  }, 905);
 
}
//win, lose, draw highlight squares
function highlightWinningSquares(winningSquares) {
  for (const selectedSquare of winningSquares) {
    const squareElement = document.getElementById('square-' + selectedSquare);
    squareElement.style.animation = 'expand 0.8s ease forwards';
    squareElement.style.color = 'rgb(18, 18, 18)';
    squareElement.style.webkitTextStroke = '1px '+ primaryColor;
    squareElement.style.textStroke = '1px '+ primaryColor;
  }
  setTimeout(() => {
    for (const selectedSquare of winningSquares) {
      const squareElement = document.getElementById('square-' + selectedSquare);
      squareElement.style.animation = 'none';
      squareElement.style.color = primaryColor;
      squareElement.style.webkitTextStroke = '';
      squareElement.style.textStroke = '';
    }
  }, 910)
}
function highlightAllSquares() {
  const squares = document.querySelectorAll(".square");
  squares.forEach((square) => {
    square.style.animation = 'expand 0.8s ease forwards';
    square.style.color = 'rgb(18, 18, 18)';
    square.style.webkitTextStroke = '1px '+ primaryColor;
    square.style.textStroke = '1px '+ primaryColor;
    setTimeout(() => {
      square.style.animation = 'none';
      square.style.color = primaryColor;
      square.style.webkitTextStroke = '';
      square.style.textStroke = '';
    }, 910);
  });
}





function bossBattleTransitions(bossHealthScreen){
  gameScreen.style.pointerEvents = 'none';
  setTimeout(() => {
    bossTransitionAudio.play()
    transitionScreen(gameScreen, bossHealthScreen);
    gameScreen.style.pointerEvents = 'all';
    clearBoard();
    setTimeout(() => {
      transitionScreen(bossHealthScreen, chooseTurnScreen);
      gameEnded = false;
      turnPlayer = remainer;
      if(bossHealth === 0){
        mode = 'easy';
        bossDiffText.textContent = 'THE BOSS IS WEAK AND BEATABLE'
      }
      else{
        bossDiffText.textContent = 'STILL IMPOSSIBLE TO WIN'
        mode = 'impossible';
      }
    }, 4000);
  }, 905);
}
function happyEndingTransition(){
  gameScreen.style.pointerEvents = 'none';
  setTimeout(() => {
    bossBeatAudio.play()
    transitionScreen(gameScreen, happyEndScreen);
    setTimeout(() => {happyEndScreen.style.opacity = "1";}, 100)
    gameScreen.style.pointerEvents = 'all';
    clearBoard();
    setTimeout(() => {
      happyEndScreen.style.opacity = "0";
      transitionScreen(happyEndScreen, chooseDifficultyScreen);
      gameEnded = false;
      bossHealth = 2;
    }, 4000);
  }, 905);
 
}