import { Component, OnInit } from '@angular/core';
import { quizQuestions } from '../../constants/quizQuestions';
import { NgIf, NgClass } from '@angular/common';

@Component({
  selector: 'app-quiz-widget',
  templateUrl: './quiz-widget.component.html',
  styleUrls: ['./quiz-widget.component.scss'],
  standalone: true,
  imports: [NgIf, NgClass],
})
export class QuizWidgetComponent implements OnInit {
  quizStarted: boolean = false;
  quizEnded: boolean = false;
  currentQuestionNumber: number = 0;
  score: number = 0;
  pickedAnswer: string | null = null;
  currentTrivia: string | null = null;
  showAnswers = false;

  questions = quizQuestions;

  ngOnInit(): void {}

  startQuiz(): void {
    this.quizStarted = true;
    this.currentQuestionNumber = 0;
    this.score = 0;
    this.currentTrivia = null;
    this.pickedAnswer = null;
    this.showAnswers = false;
  }

  selectAnswer(option: string): void {
    this.pickedAnswer = option;
    this.showAnswers = true;
    this.currentTrivia = this.questions[this.currentQuestionNumber].trivia;

    if (option === this.questions[this.currentQuestionNumber].answer) {
      this.score++;
    }
  }

  nextQuestion(): void {
    this.currentQuestionNumber++;
    this.pickedAnswer = null;
    this.showAnswers = false;
    this.currentTrivia = null;
    if (this.currentQuestionNumber >= this.questions.length) {
      this.quizStarted = false;
      this.quizEnded = true;
    }
  }

  getOptionClass(option: string): string {
    if (!this.showAnswers) {
      return '';
    }
    if (option === this.pickedAnswer) {
      return option === this.questions[this.currentQuestionNumber].answer
        ? 'correct'
        : 'incorrect';
    }
    if (option === this.questions[this.currentQuestionNumber].answer) {
      return 'correct';
    }
    return '';
  }

  restartQuiz(): void {
    this.quizEnded = false;
  }
}
