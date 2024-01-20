import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Member } from 'src/app/_models/Member';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css'],
})
export class MemberCardComponent implements OnInit {
  constructor(
    private memberService: MembersService,
    private toastr: ToastrService
  ) {}
  ngOnInit(): void {}
  @Input() member: Member;

  addLike(member: Member) {
    var r: string;
    r = member.userName;
    this.memberService.addLike(r).subscribe(() => {
      this.toastr.success('You have liked ' + member.knownAs);
    });
  }
}
