//for the game to start
const fullContainer = document.querySelector('#full-container');
const startBtn = document.querySelector('.start-btn');
const overlay = document.querySelector('#overlay');
const gameContainer = document.querySelector('#game-container');
const textBelowTitle = document.querySelector('#text-below-title');
const title = document.querySelector('#title');
const titleR = document.querySelector('.title-r');
const titleP = document.querySelector('.title-p');
const titleS = document.querySelector('.title-s');
const choices = document.querySelector('#choices-container');
const scoresContainer = document.querySelector('#scores');
const fullscreenButton = document.querySelector('#fullscreen-button');
const whoosh = document.getElementById('whoosh');
const winAudio = document.getElementById('win-audio');
const loseAudio = document.getElementById('lose-audio');
const rotate = document.getElementById('rotate');


//transition to the game from start game screen
startBtn.addEventListener('click', hideOverlayAndGameStart);
function hideOverlayAndGameStart()
{
    gameContainer.style.opacity = '1';
    overlay.style.opacity = '0';
    setTimeout(() => {
        overlay.style.display = "none";
        gameContainer.style.display = 'grid';
        disablePointers();
        setTimeout(() => {
            titleR.style.opacity = '1';
            setTimeout(() => {
                titleP.style.opacity = '1'; //I know I am nesting a lot, will improve on this in the next projects
                setTimeout(() => {
                    titleS.style.opacity = '1';
                    setTimeout(() => {
                        textBelowTitle.style.opacity = '1';
                        setTimeout(() => {
                            scoresContainer.style.opacity = '1';
                            choices.style.opacity = '1';
                            enablePointers();
                        }, 1200)    
                    }, 1000)
                }, 700)
            }, 700);
          }, 700);
        }, 600);
}

let playerScore = 0;
let computerScore = 0;
const rButton = document.querySelector('#rock');
const pButton = document.querySelector('#paper');
const sButton = document.querySelector('#scissors');
let playerScoreSpan = document.querySelector('#player-score-span');
let computerScoreSpan = document.querySelector('#computer-score-span');

function playerSelection(){
    rButton.addEventListener('click', () => {
        playRound('rock', getComputerChoice())
    });
    pButton.addEventListener('click', () => {
        playRound('paper', getComputerChoice())
    });
    sButton.addEventListener('click', () => {
        playRound('scissors', getComputerChoice())
    });
}

function getComputerChoice(){
let random_number = Math.floor(Math.random() * 3);
let  ComputerSelection;
    if(random_number == 0){
    return ComputerSelection = "rock";
}
    else if(random_number == 1){
    return ComputerSelection = "paper";
}
    else if(random_number == 2){
    return ComputerSelection = "scissors";
}

}
function playRound(x, y){
    if (x == "rock" && y == "paper"){
        return loseRP();
    }
    else if (x == "rock" && y == "scissors"){
        return winRS();
    }
    else if (x == "paper" && y == "scissors"){
        return losePS();
    }
    else if (x == "paper" && y == "rock"){
        return winPR();
    }
    else if (x == "scissors" && y == "rock"){
        return loseSR();;
    }
    else if (x == "scissors" && y == "paper"){
        return winSP();
    }
    else if (x == "rock" && y == "rock"){
        return tieR();
    }
    else if (x == "paper" && y == "paper"){
        return tieP();
    }
    else if (x == "scissors" && y == "scissors"){
        return tieS();
    }
  
}

const animationsContainer = document.querySelector('#animations-container')
let imageLeft = document.getElementById("left-png");
let imageRight = document.getElementById("right-png");

function animation(left, right){

setTimeout(() => {
whoosh.play();
imageLeft.src = left;
imageRight.src = right;
animationsContainer.style.display = 'grid';
gameContainer.style.display = 'none';
setTimeout(() => {
    imageLeft.style.marginLeft = '11vw'
imageRight.style.marginRight = '11vw'
}, 50)
disablePointers();
setTimeout(() => {
    enablePointers();
    animationsContainer.style.display = 'none';
    gameContainer.style.display = 'grid';
    imageLeft.style.marginLeft = '4vw'
    imageRight.style.marginRight = '4vw'
}, 1900)
}, 100)
}

function win(){
    setTimeout(() => {
        playerScore++;
        playerScoreSpan.innerHTML = playerScore;
        computerScoreSpan.innerHTML = computerScore; 
            setTimeout(() => {
                if(playerScore == 3){
                    disablePointers();
                    gameReset();
                        winnerF();
                }
            }, 10); 
        
        
    }, 2000)

}
function lose(){
    setTimeout(() => {
        computerScore++;
        playerScoreSpan.innerHTML = playerScore;
        computerScoreSpan.innerHTML = computerScore;
        setTimeout(() => {
            if(computerScore == 3){
                disablePointers();
                gameReset();
                    loserF();
            }
        }, 10); 
        
    }, 2000)
   
}
function gameReset(){
    playerScore = 0;
    computerScore = 0;
    playerScoreSpan.innerHTML = playerScore;
    computerScoreSpan.innerHTML = computerScore;
}
const R = "img/rock.png";
const R2 = "img/rock-right.png";
const P = "img/paper.png";
const P2 = "img/paper-right.png";
const S = "img/scissors.png";
const S2 = "img/scissors-right.png";

function loseRP(){
    lose();
    animation(R, P2);
}
function winPR(){
    win();
    animation(P, R2);
}
function winSP(){
    win();
    animation(S, P2);
}

function losePS(){
    lose();
    animation(P, S2);
}
function winRS(){
    win();
    animation(R, S2);
}
function loseSR(){
    lose();
    animation(S, R2);
}
function tieR(){
    animation(R, R2);
}
function tieP(){
    animation(P, P2);
}
function tieS(){
    animation(S, S2);
}



const winner = document.querySelector('#winner-screen');
const loser = document.querySelector('#loser-screen');
const allBtn = document.querySelector('.buttons');
const youWin = document.querySelector('#you-win');
const youLose = document.querySelector('#you-lose');
const doAgain = document.querySelector('#now-do-it-again');
const tryAgain = document.querySelector('#better-luck-next-time');

function winnerF(){
gameContainer.style.display = 'none';
setTimeout(() => {
    winner.style.display = 'flex';
}, 300);
setTimeout(() => {
    winAudio.play();
    youWin.style.opacity = '1';
}, 1000);
setTimeout(() => {
    doAgain.style.opacity = '1';
}, 2000);
setTimeout(() => {
    youWin.style.opacity = '0';
    doAgain.style.opacity = '0';
    setTimeout(() => {
        winner.style.display = 'none';
    gameContainer.style.display = 'grid';
    enablePointers()
    }, 200);
}, 5000);
}
function loserF(){
    gameContainer.style.display = 'none';
    setTimeout(() => {
        loser.style.display = 'flex';
    }, 300);
    setTimeout(() => {
        loseAudio.play();
        youLose.style.opacity = '1';
    }, 1000);
    setTimeout(() => {
        tryAgain.style.opacity = '1';
    }, 3000);
    setTimeout(() => {
    youLose.style.opacity = '0';
    tryAgain.style.opacity = '0';
    setTimeout(() => {
        loser.style.display = 'none';
        gameContainer.style.display = 'grid';
        enablePointers()
    }, 200);
    }, 5000);
}
function disablePointers(){
    gameContainer.style.pointerEvents = 'none';
    allBtn.style.cursor = 'none';
}
function enablePointers(){
    gameContainer.style.pointerEvents = 'auto';
    allBtn.style.cursor = 'pointer';
}

//fullscreen button

fullscreenButton.addEventListener('click', function() {
    if (!document.fullscreenElement && !document.webkitFullscreenElement && !document.mozFullScreenElement && !document.msFullscreenElement) {
      // Enter fullscreen
      if(window.matchMedia('(max-width: 1100px)').matches || window.matchMedia('(min-height: 2500px)').matches){
        rotate.style.display = 'flex';
        setTimeout(() => {
          rotate.style.opacity = '1';
        }, 100)
        setTimeout(() => {
          rotate.style.display = 'none';
          rotate.style.opacity = '0';
        }, 2000)
      }
      if (fullContainer.requestFullscreen) {
        fullContainer.requestFullscreen();
      } else if (fullContainer.mozRequestFullScreen) {
        fullContainer.mozRequestFullScreen();
      } else if (fullContainer.webkitRequestFullscreen) {
        fullContainer.webkitRequestFullscreen();
      } else if (fullContainer.msRequestFullscreen) {
        fullContainer.msRequestFullscreen();
      }
    } else {
      // Exit fullscreen
      if (document.exitFullscreen) {
        document.exitFullscreen();
      } else if (document.mozCancelFullScreen) {
        document.mozCancelFullScreen();
      } else if (document.webkitExitFullscreen) {
        document.webkitExitFullscreen();
      } else if (document.msExitFullscreen) {
        document.msExitFullscreen();
      }
    }
  });

// audio buttons
let gameAudio = document.querySelectorAll('.game-audio');
let muteButton = document.querySelector('.mute-png')
let isMuted = false;

muteButton.addEventListener('click', function() {
  isMuted = !isMuted;

  gameAudio.forEach(function(audio) {
    audio.muted = isMuted; 
  });

  muteButton.setAttribute('src', isMuted ? 'img/muted.png' : 'img/unmuted.png'); 
});




playerSelection();