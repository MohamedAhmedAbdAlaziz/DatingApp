import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map, of, reduce, take } from 'rxjs';
import { environment } from 'src/environments/environment.development';
import { Member } from '../_models/Member';
import { PaginationResult } from '../_models/pagination';
import { UserParams } from '../_models/userParams';
import { User } from '../_models/User';
import { AccountService } from './account.service';
// const httpOption = {
//   headers: new HttpHeaders({
//     Authorization: 'Bearer ' + JSON.parse(localStorage.getItem('user')).token,
//   }),
// };

@Injectable({
  providedIn: 'root',
})
export class MembersService {
  baseUrl = environment.apiUrl;
  members: Member[] = [];
  userParams: UserParams;
  user: User;
  paginationResult: PaginationResult<Member[]> = new PaginationResult<
    Member[]
  >();
  memberCache = new Map();
  constructor(
    private http: HttpClient,
    private accountService: AccountService
  ) {
    this.accountService.currentUser$.pipe(take(1)).subscribe((user) => {
      this.user = user;
      this.userParams = new UserParams(user);
    });
  }
  getUSerParams() {
    return this.userParams;
  }
  setUserParams(params: UserParams) {
    this.userParams = params;
  }
  resetUserParams() {
    this.userParams = new UserParams(this.user);
    return this.userParams;
  }
  // getMembers(): Observable<Member[]> {
  //   // if (this.members.length > 0) return of(this.members);
  //   return this.http
  //     .get<Member[]>(this.baseUrl + 'users')
  //     .pipe(
  //     map((members) => {
  //       this.members = members;
  //       return members;
  //     })
  //     );
  //   //return this.http.get<Member[]>(this.baseUrl + 'users', httpOption);
  // }
  private getPaginationHeaders(pageNumber: number, pageSize: number) {
    let params = new HttpParams();
    params = params.append('pageNumber', pageNumber.toString());
    params = params.append('pageSize', pageSize.toString());

    return params;
  }

  getMembers(userParams: UserParams) {
    // let params = new HttpParams();
    var response = this.memberCache.get(Object.values(userParams).join('-'));
    if (response) {
      return of(response);
    }
    console.log(Object.values(userParams).join('-'));
    let params = this.getPaginationHeaders(
      userParams.pageNumber,
      userParams.pageSize
    );

    params = params.append('minAge', userParams.minAge.toString());
    params = params.append('maxAge', userParams.maxAge.toString());
    params = params.append('gender', userParams.gender);
    params = params.append('orderBy', userParams.orderBy);

    return this.getPaginationResult<Member[]>(
      this.baseUrl + 'users',
      params
    ).pipe(
      map((response) => {
        debugger;
        this.memberCache.set(Object.values(userParams).join('-'), response);
        console.log(this.memberCache.values());
        return response;
      })
    );
  }

  private getPaginationResult<T>(url, params) {
    const paginationResult: PaginationResult<T> = new PaginationResult<T>();
    return this.http.get<T>(url, { observe: 'response', params }).pipe(
      map((response) => {
        debugger;
        paginationResult.result = response.body;
        if (response.headers.get('Pagination') !== null) {
          paginationResult.pagination = JSON.parse(
            response.headers.get('Pagination')
          );
        }
        return paginationResult;
      })
    );
  }

  getMember(username: string): Observable<Member> {
    // return this.http.get<Member>(
    //   this.baseUrl + 'users/' + username,
    //   httpOption
    // );
    const members = [...this.memberCache.values()]
      .reduce((arr, elem) => arr.concat(elem.result), [])
      .find((member: Member) => member.userName === username);

    // const member = this.members.find((x) => x.userName === username);
    // if (member !== undefined) return of(member);
    return this.http.get<Member>(this.baseUrl + 'users/' + username);
  }

  updateMember(member: Member) {
    return this.http.put(this.baseUrl + 'users', member).pipe(
      map(() => {
        const index = this.members.indexOf(member);
        this.members[index] = member;
      })
    );
  }
  setMainPhoto(photoId: number) {
    return this.http.put(this.baseUrl + 'users/set-main-photo/' + photoId, {});
  }
  deletePhoto(photoId: number) {
    return this.http.delete(this.baseUrl + 'users/delete-photo/' + photoId);
  }
}
