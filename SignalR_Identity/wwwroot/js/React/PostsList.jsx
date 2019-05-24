 class PostsList extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            userId: props.userId,
            posts: []
        }
        this.removeHandler = this.removeHandler.bind(this);
    }

     addNewPost(post) {
         console.log(post);
        this.state.posts.unshift(post);
        this.setState({ state: this.state });
    }

     render() {
        const posts = this.state.posts.map(post => (
            <Post id={post.id}
                text={post.text}
                date={post.date}
                liked={post.liked}
                userAuth={post.userAuth}
                authorName={post.authorName}
                authorAvatarPath={post.authorAvatarPath}
                isOwner={post.isOwner}
                removeHandler={this.removeHandler}
                key={post.id}
            >
            </Post>
        ));

        return <div id="feedList">
                   <ul className="list-group">
                       {posts}
                   </ul>
               </div>;

     }

     removeHandler(postId) {
         this.setState({ posts: this.state.posts.filter(el => el.id !== postId) });

     }

    componentWillMount() {
        this.getItems();
    }


    getItems() {
        fetch('https://localhost:44358/api/Posts/GetPostsByUserId/' + this.state.userId)
            .then(results => results.json())
            .then(results => {
                this.setState({ posts: results });
            });
    }
}
