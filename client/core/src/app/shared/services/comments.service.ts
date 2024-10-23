import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Observable, map } from 'rxjs';
import { Comment, CommentUploadDto } from '../models/comments';
import { PaginatedResult, Pagination } from '../models/pagination';

@Injectable({
  providedIn: 'root',
})
export class CommentsService {
  baseUrl = environment.apiUrl;
  paginatedResult: PaginatedResult<Comment[]> = new PaginatedResult<
    Comment[]
  >();

  constructor(private http: HttpClient) {}

  getPostComments(
    postId: number,
    pageNumber: number,
    pageSize: number
  ): Observable<PaginatedResult<Comment[]>> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    return this.http
      .get<{ comments: Comment[]; pagination: Pagination }>(
        this.baseUrl + 'comments/' + postId,
        {
          observe: 'response',
          params,
        }
      )
      .pipe(
        map((response) => {
          this.paginatedResult.result = response.body?.comments;
          if (response.headers.get('Pagination')) {
            this.paginatedResult.pagination = JSON.parse(
              response.headers.get('Pagination') as string
            );
          }
          return this.paginatedResult;
        })
      );
  }

  uploadComment(commentUploadDto: CommentUploadDto): Observable<Comment> {
    return this.http.post<Comment>(this.baseUrl + 'comments', commentUploadDto);
  }

  deleteComment(commentId: number): Observable<any> {
    return this.http.delete(this.baseUrl + 'comments/' + commentId);
  }
}
