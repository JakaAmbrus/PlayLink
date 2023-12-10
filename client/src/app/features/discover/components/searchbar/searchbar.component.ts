import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { SearchUser } from 'src/app/shared/models/users';
import { UsersService } from 'src/app/shared/services/users.service';

@Component({
  selector: 'app-searchbar',
  templateUrl: './searchbar.component.html',
  styleUrl: './searchbar.component.scss',
})
export class SearchbarComponent implements OnInit {
  searchUsers: SearchUser[] = [];
  filteredUsers: SearchUser[] = [];
  searchControl = new FormControl();

  constructor(private usersService: UsersService) {}

  ngOnInit(): void {
    this.searchControl.valueChanges.subscribe((val) => {
      this.filterUsers(val);
    });

    this.loadSearchUsers();
  }

  filterUsers(val: string) {
    if (!val) {
      this.filteredUsers = this.searchUsers;
    } else {
      this.filteredUsers = this.searchUsers.filter((user) =>
        user.fullName.toLowerCase().includes(val.toLowerCase())
      );
    }
  }

  loadSearchUsers() {
    this.usersService.getSearchUsers().subscribe({
      next: (response) => {
        if (response) {
          this.searchUsers = response;
        }
      },
    });
  }
}
